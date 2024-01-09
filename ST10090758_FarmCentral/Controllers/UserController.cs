using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ST10090758_FarmCentral.Database;
using ST10090758_FarmCentral.Models;
using System.Security;

namespace ST10090758_FarmCentral.Controllers
{
    public class UserController : Controller
    {
        public static string typeOfUser;
        public static DBoperations dbOp;
        public UserController(IConfiguration configuration)
        {
            dbOp= new DBoperations(configuration);//establish connection
        }
        //login
        public ActionResult Login()
        {
            return View();
        }

        //LOGIN VALIDATION
        // use HttpPost to get actions from forms/the view. Also make the method IActionResult
        [HttpPost]
        public IActionResult ValidateLogin()
        {
            User u;
            //retrieve data from user
            string username = Request.Form["txtUserName"].ToString();// convert form to string. Numbers must be converted too
            string password = Request.Form["txtPassword"].ToString();

            
            //pass the user name to the method to retieve the user if it exists
           
            try
            {
                if (!username.Equals("")|| !password.Equals(""))//only execute if the user has entered credentials
                {
                    dbOp.getUserLogin(username);
                    if (dbOp.UserName.Equals(username) && dbOp.Password.Equals(password) && dbOp.UserType.Equals("EMPLOYEE"))
                    {
                        return View("Index", dbOp.allUsers());
                    }
                    else if (dbOp.UserName.Equals(username) && dbOp.Password.Equals(password) && dbOp.UserType.Equals("FARMER"))
                    {
                        return RedirectToAction("FarmerProduct", "Product");
                    }
                    else
                    {
                        return View("Login");
                    }

                }
                return View("Login");
            }catch(Exception e)
            {
                return View("Login");
            }
            
        }

        // GET: UserController
        public ActionResult Index()
        {
            List<User> users;
            
            //return index view with a list of all users
            return View(dbOp.allUsers());
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            User u = dbOp.getUserDetails(id);
            return View(u);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            

            try
            {
                //obtain user input from form
                //int userID = Convert.ToInt32(collection["txtUserID"]);
                string name = collection["txtName"];
                string surname = collection["txtSurname"];
                string userType = collection["txtUserType"];
                string userName = collection["txtUserName"];
                string password = collection["txtUserPassword"];
                //using input create User object that will be sent as a paramter to add a student to the db
                User u = new User(name,surname,userType,userName,password);

                dbOp.addUser(u);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            User u = dbOp.getUserDetails(id);//retrieve the user's profile to view it before deletion

            return View(u);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                dbOp.editUser(id,collection["lblName"], collection["lblSurname"], collection["lblUserType"], collection["lblUserName"], collection["lblPassword"]);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            User u =dbOp.getUserDetails(id);//retrieve the user's profile to view it before deletion

            return View(u);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
               
                dbOp.deleteUser(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
            
        }
    }
}
