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
    public async Task<ReviewModel> CreateReview(ReviewModel review)
    {
        try
        {
            var reviews = await GetReviewsByProperty(x => x.ArticleNumber == review.ArticleNumber);

            if (!reviews.Any(x => x.Author == review.Author))
            {
                var result = await _reviewRepository.CreateAsync(new ProductReviewEntity()
                {
                    Review = review.Review,
                    Rating = review.Rating,
                    Author = review.Author,
                    ArticleNumber = review.ArticleNumber,
                });

                if (result != null!)
                {
                    return new ReviewModel()
                    {
                        Id = result.Id,
                        Review = result.Review,
                        Rating = result.Rating,
                        Author = result.Author,
                        ArticleNumber = result.ArticleNumber,
                    };
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
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

            if (entityList != null)
            {
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
                        }); ;
                    }
                }
                return reviewList;
            }
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
