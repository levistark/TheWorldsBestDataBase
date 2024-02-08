using System.Diagnostics;
using TWBD_Domain.DTOs.Models.Product;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Domain.Services.ProductServices;
public class ReviewService
{

    private readonly ProductReviewRepository _reviewRepository;
    public ReviewService(ProductReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<List<ReviewModel>> GetAllReviews()
    {
        try
        {
            List<ReviewModel> reviewList = [];
            var enttityList = await _reviewRepository.ReadAllAsync();

            foreach (var entity in enttityList)
            {
                reviewList.Add(new ReviewModel()
                {
                   Id = entity.Id,
                   Review = entity.Review,
                   Rating = entity.Rating,
                   Author = entity.Author,
                   ArticleNumber = entity.ArticleNumber,
                });;
            }

            return reviewList;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public async Task<List<ReviewModel>> GetReviewsByProperty(Func<ProductReviewEntity, bool> predicate)
    {
        try
        {
            List<ReviewModel> reviewList = [];

            var entityList = await _reviewRepository.ReadAllAsync();

            foreach (var entity in entityList)
            {
                if (predicate(entity))
                {
                    reviewList.Add(new ReviewModel()
                    {
                        Id = entity.Id,
                        Review = entity.Review,
                        Rating = entity.Rating,
                        Author = entity.Author,
                        ArticleNumber = entity.ArticleNumber,
                    });;
                }
            }
            return reviewList;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
    public async Task<bool> DeleteReviewById(int id)
    {
        try
        {
            var reviewToDelete = await _reviewRepository.ReadOneAsync(x => x.Id == id);

            if (reviewToDelete != null)
            {
                return await _reviewRepository.DeleteAsync(x => x.Id == reviewToDelete.Id, reviewToDelete);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }
}
