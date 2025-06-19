using Microsoft.AspNetCore.Mvc;
using NewsNotifier.Interfaces;
using NewsNotifier.Models.Entities;
using NewsNotifier.Repositories.Interfaces;
using System;

namespace NewsNotifier.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _repo;

        public NewsArticleService(INewsArticleRepository repo)
        {
            _repo = repo;
        }

        public Task<NewsArticle?> GetNewsByIdAsync(int id) => _repo.GetByIdAsync(id);

        public Task<IEnumerable<NewsArticle>> GetAllNewsAsync() => _repo.GetAllAsync();

        public Task CreateNewsAsync(NewsArticle article) => _repo.AddAsync(article);

        public Task UpdateNewsAsync(NewsArticle article) => _repo.UpdateAsync(article);

        public Task DeleteNewsAsync(int id) => _repo.DeleteAsync(id);
    }
}
