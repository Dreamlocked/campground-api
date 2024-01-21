﻿using campground_api.Models;
using campground_api.Models.Dto;

namespace campground_api.Utils
{
    public static class Mapper
    {
        public static CampgroundGetDto MapCampgroundToCampgroundGetDto(Campground campground)
        {
            var campgroundGetDto = new CampgroundGetDto()
            {
                Id = campground.Id,
                Title = campground.Title,
                Description = campground.Description,
                Location = campground.Location,
                Latitude = campground.Latitude,
                Longitude = campground.Longitude,
                Price = campground.Price,
                Images = campground.Images.Select(image => new ImageDto()
                {
                    Filename = image.Filename,
                    Url = image.Url
                }).ToList(),
                Reviews = campground.Bookings.Select(booking => 
                booking.Reviews.Select(review => new ReviewListDto()
                {
                    Id = review.Id,
                    Body = review.Body,
                    Rating = review.Rating,
                    User = new UserDto()
                    {
                        Id = booking.User.Id,
                        Username = booking.User.Username,
                        Email = booking.User.Email,
                        FirstName = booking.User.FirstName,
                        LastName = booking.User.LastName
                    }
                })
                ).SelectMany(x => x).ToList(),
                Host = new UserDto()
                {
                    Id = campground.Host.Id,
                    Username = campground.Host.Username,
                    Email = campground.Host.Email,
                    FirstName = campground.Host.FirstName,
                    LastName = campground.Host.LastName
                }
            };

            return campgroundGetDto;
        }

        public static CampgroundListDto MapCampgroundToCampgroundListDto(Campground campground)
        {
            var allRatings = campground.Bookings.Select(booking => booking.Reviews.Select(review => (int) review.Rating)).SelectMany(x => x);

            double? score = allRatings.Any() ? allRatings.Average() : null;

            var campgroundListDto = new CampgroundListDto()
            {
                Id = campground.Id,
                Title = campground.Title,
                Description = campground.Description,
                Location = campground.Location,
                Price = campground.Price,
                Images = campground.Images.Select(image => new ImageDto()
                {
                    Filename = image.Filename,
                    Url = image.Url
                }).ToList(),
                Score = score
            };

            return campgroundListDto;
        }

        public static ReviewListDto MapReviewToReviewListDto(Review review)
        {
            var reviewListDto = new ReviewListDto()
            {
                Id = review.Id,
                Body = review.Body,
                Rating = review.Rating,
                User = new UserDto()
                {
                    Id = review.Bookings.User.Id,
                    Username = review.Bookings.User.Username,
                    Email = review.Bookings.User.Email,
                    FirstName = review.Bookings.User.FirstName,
                    LastName = review.Bookings.User.LastName
                }
            };

            return reviewListDto;
        }
    }
}