namespace TechnicalTest.Services;

using AutoMapper;
using BCrypt.Net;
using TechnicalTest.Authorization;
using TechnicalTest.Entities;
using TechnicalTest.Helpers;
using TechnicalTest.Models.Users;

public interface IUserService
{
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    IEnumerable<User> GetUsers();
    User GetById(int id);
    void Register(RegisterRequest model);
    void Update(int id, UpdateRequest model);
    void Delete(int id);
}

public class UserService : IUserService
{
    private DataContext _dataContext;
    private IJwtUtils _jwtUtils;
    private readonly IMapper _mapper;

    public UserService(DataContext dataContext, IJwtUtils jwtUtils, IMapper mapper)
    {
        _dataContext = dataContext;
        _jwtUtils = jwtUtils;
        _mapper = mapper;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        var user = _dataContext.Users.SingleOrDefault(x => x.Username == model.Username);

        if (user == null || !BCrypt.Verify(model.Password, user.PasswordHash)) throw new AppException("Username or Password is incorrect");

        var response = _mapper.Map<AuthenticateResponse>(user);
        response.Token = _jwtUtils.GenerateToken(user);
        return response;
    }

    public IEnumerable<User> GetUsers() { return _dataContext.Users; }

    public User GetById(int id) { return getUser(id); }

    public void Register(RegisterRequest model)
    {
        if (_dataContext.Users.Any(x => x.Username == model.Username)) throw new AppException("Username '" + model.Username + "' is already taken");

        var user = _mapper.Map<User>(model);
        user.PasswordHash = BCrypt.HashPassword(model.Password);
        _dataContext.Users.Add(user);
        _dataContext.SaveChanges();
    }

    public void Update(int id, UpdateRequest model)
    {
        var user = getUser(id);

        if (model.Username != user.Username && _dataContext.Users.Any(x => x.Username == model.Username)) throw new AppException("Username '" + model.Username + "' is already taken");

        if (!string.IsNullOrEmpty(model.Password)) user.PasswordHash = BCrypt.HashPassword(model.Password);

        _mapper.Map(model, user);
        _dataContext.Users.Update(user);
        _dataContext.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = getUser(id);
        _dataContext.Users.Remove(user);
        _dataContext.SaveChanges();
    }

    private User getUser(int id)
    {
        var user = _dataContext.Users.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found.");
        return user;
    }
}
