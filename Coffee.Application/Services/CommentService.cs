using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastFood.Application.DTOs;
using FastFood.Application.Interfaces;
using FastFood.Domain.Entities;
using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Repositories;

namespace FastFood.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly INotificationService _notificationService;

        public CommentService(ICommentRepository commentRepository, INotificationService notificationService)
        {
            _commentRepository = commentRepository;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByProductIdAsync(int productId)
        {
            var comments = await _commentRepository.GetCommentsByProductIdAsync(productId);
            // Include replies logic if needed, or handle in repository
            return comments.Select(MapToDto);
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByComboIdAsync(int comboId)
        {
            var comments = await _commentRepository.GetCommentsByComboIdAsync(comboId);
            return comments.Select(MapToDto);
        }

        public async Task<CommentDto> CreateCommentAsync(int userId, int? productId, int? comboId, string content, int rating, int? parentId = null)
        {
            var comment = new Comment
            {
                UserId = userId,
                ProductId = productId,
                ComboId = comboId,
                Content = content,
                Rating = rating,
                ParentId = parentId,
                CreatedAt = DateTime.Now
            };

            await _commentRepository.AddAsync(comment);

            // Handle notification if it's a reply
            if (parentId.HasValue)
            {
                var parentComment = await _commentRepository.GetByIdAsync(parentId.Value);
                if (parentComment != null && parentComment.UserId != userId)
                {
                    var link = comboId.HasValue ? $"/Guest/Combo/Detail/{comboId}" : $"/Guest/Product/Detail/{productId}";
                    await _notificationService.CreateNotificationAsync(
                        parentComment.UserId,
                        "CommentReply",
                        "Phản hồi mới",
                        $"Bạn có một phản hồi mới cho bình luận của mình.",
                        link
                    );
                }
            }

            return MapToDto(comment);
        }

        public async Task<bool> HideCommentAsync(int commentId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null) return false;

            comment.IsHidden = true;
            await _commentRepository.UpdateAsync(comment);
            return true;
        }

        public async Task<IEnumerable<CommentDto>> GetAllCommentsAsync()
        {
            var comments = await _commentRepository.GetVisibleCommentsAsync();
            return comments.Select(MapToDto);
        }

        public async Task<bool> DeleteCommentAsync(int commentId, int requestingUserId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null) return false;
            // Only the comment owner can delete their own comment
            if (comment.UserId != requestingUserId) return false;

            comment.IsHidden = true; // Soft delete
            await _commentRepository.UpdateAsync(comment);
            return true;
        }

        private CommentDto MapToDto(Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                UserId = comment.UserId,
                UserName = comment.User?.FullName ?? "User",
                ProductId = comment.ProductId,
                ProductName = comment.Product?.Name,
                ComboId = comment.ComboId,
                ComboName = comment.Combo?.Name,
                Content = comment.Content,
                Rating = comment.Rating,
                IsHidden = comment.IsHidden,
                CreatedAt = comment.CreatedAt,
                ParentId = comment.ParentId,
                Replies = comment.Replies
                    .Where(r => !r.IsHidden)
                    .Select(r => new CommentDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        UserName = r.User?.FullName ?? "User",
                        Content = r.Content,
                        Rating = r.Rating,
                        IsHidden = r.IsHidden,
                        CreatedAt = r.CreatedAt,
                        ParentId = r.ParentId
                    }).ToList()
            };
        }
    }
}
