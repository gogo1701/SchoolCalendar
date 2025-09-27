using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Caching.Memory;
using SchoolProject.Models;
using SchoolProject.Models.Interfaces;

namespace SchoolProject.Services;

public class NewsService
{
    private readonly INewsRepository _newsRepository;
    private readonly IMemoryCache _cache;
    private const string FirstPageCacheKey = "news_first_page";
    public NewsService(INewsRepository newsRepository, IMemoryCache cache)
    {
        _newsRepository = newsRepository;
        _cache = cache; 
    }

    public async Task<IEnumerable<News>> GetPaginatedNewsAsync(int page, int pageSize)
    {
        if (page == 1 && _cache.TryGetValue(FirstPageCacheKey, out IEnumerable<News> cachedFirstPage))
        {
            return cachedFirstPage;
        }

        var allNews = await _newsRepository.GetAllAsync();
        var paged = allNews
            .OrderByDescending(n => n.DatePublished)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        if (page == 1)
        {
            _cache.Set(FirstPageCacheKey, paged, TimeSpan.FromSeconds(30));
        }

        return paged;
    }

    public async Task<int> GetTotalNewsCountAsync()
    {
        var all = await _newsRepository.GetAllAsync();
        return all.Count();
    }

    public async Task<IEnumerable<News>> GetAllNewsAsync()
    {
        return await _newsRepository.GetAllAsync();
    }

    public async Task<News?> GetNewsByIdAsync(int id)
    {
        return await _newsRepository.GetByIdAsync(id);
    }

    public async Task CreateNewsAsync(News news)
    {
        news.DatePublished = DateTime.UtcNow;
        await _newsRepository.AddAsync(news);
    }

    public async Task UpdateNewsAsync(News news)
    {
        await _newsRepository.UpdateAsync(news);
    }

    public async Task DeleteNewsAsync(int id)
    {
        await _newsRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<CalendarItem>> GetAllCalendarItemsAsync()
    {
        const string cacheKey = "CalendarItemsCache";

        if (_cache.TryGetValue(cacheKey, out IEnumerable<CalendarItem> cachedItems))
            return cachedItems;

        var newsWithCalendar = (await _newsRepository.GetAllAsync())
            .Where(x => x.DateOnCalendar.HasValue);

        var calendarItems = newsWithCalendar.Select(x => new CalendarItem
        {
            Id = x.Id,
            Title = x.Title,
            CalendarDate = x.DateOnCalendar!.Value
        }).ToList();

        _cache.Set(cacheKey, calendarItems, TimeSpan.FromSeconds(30));

        return calendarItems;
    }

}