using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Core.Exceptions.DataAccessExceptions;
using TaskTracker_DAL.Entities;
using TaskTracker_DAL.GenericRepository;

namespace TaskTracker_BL.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IGenericRepository<UserImage> _genericUserImageRepository;
        private readonly IGenericRepository<User> _genericUserRepository;
        public ImageService(
            IGenericRepository<UserImage> genericUserImageRepository,
            IGenericRepository<User> genericUserRepository)
        {
            _genericUserImageRepository = genericUserImageRepository;
            _genericUserRepository = genericUserRepository;
        }

        public async Task<bool> AddImageAsync(IFormFile uploadedFile, string email)
        {
            if (uploadedFile != null)
            {
                string path = @"D:\Files\" + uploadedFile.FileName;

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                User user = _genericUserRepository.GetByPredicate(x => x.Email == email).FirstOrDefault();

                var userImage = await _genericUserImageRepository.GetByPredicate(u => u.UserId == user.Id).FirstOrDefaultAsync();

                if (userImage != null)
                {
                    _genericUserImageRepository.DeleteAsync(userImage.Id);
                }

                UserImage file = new UserImage { FileName = uploadedFile.FileName, FilePath = path, User = user, UserId = user.Id };

                file.Id = Guid.NewGuid();

                await _genericUserImageRepository.CreateAsync(file);

                return true;
            }

            throw new ObjectNotFoundException(typeof(IFormFile));
        }
    }
}
