using Microsoft.EntityFrameworkCore;
using TWBD_Domain.DTOs;

namespace TWBD_Domain.Services;
public class UserRegisterService
{

    //public void RegisterUser(UserRegistrationModel newUser)
    //{
    //    // validate
    //    if (_context.Users.Any(x => x.Username == model.Username))
    //        throw new AppException("Username '" + model.Username + "' is already taken");

    //    // map model to new user object
    //    var user = _mapper.Map<User>(model);

    //    // hash password
    //    user.PasswordHash = BCrypt.HashPassword(model.Password);

    //    // save user
    //    _context.Users.Add(user);
    //    _context.SaveChanges();
    //}

    //public void Update(int id, UpdateRequest model)
    //{
    //    var user = getUser(id);

    //    // validate
    //    if (model.Username != user.Username && _context.Users.Any(x => x.Username == model.Username))
    //        throw new AppException("Username '" + model.Username + "' is already taken");

    //    // hash password if it was entered
    //    if (!string.IsNullOrEmpty(model.Password))
    //        user.PasswordHash = BCrypt.HashPassword(model.Password);

    //    // copy model to user and save
    //    _mapper.Map(model, user);
    //    _context.Users.Update(user);
    //    _context.SaveChanges();
    //}

    //public void Delete(int id)
    //{
    //    var user = getUser(id);
    //    _context.Users.Remove(user);
    //    _context.SaveChanges();
    //}

    //// helper methods

    //private User getUser(int id)
    //{
    //    var user = _context.Users.Find(id);
    //    if (user == null) throw new KeyNotFoundException("User not found");
    //    return user;
    //}
}
