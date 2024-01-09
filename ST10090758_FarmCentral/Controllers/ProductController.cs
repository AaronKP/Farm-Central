using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ST10090758_FarmCentral.Controllers;
using ST10090758_FarmCentral.Database;
using ST10090758_FarmCentral.Models;

namespace ST10090758_FarmCentral.Controllers
{
    public class ProductController : Controller
    {
        // GET: ProductController
        
        public ActionResult Product(int id)
        {
            if (UserController.dbOp.UserID == 0)
            {
                UserController.dbOp.UserID = id;
            }
                
            return View(UserController.dbOp.allProducts(UserController.dbOp.UserID));
        }

        // this action is used as an aid to help the product action when it no longer has the id.
        public ActionResult FarmerProduct(int Farmerid)//where farmerid is the static userID
        {
            return View("Product", UserController.dbOp.allProducts(DBoperations.myUserId));//ref Product view and supply model using static id
        }

        //filtering product by type
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FilterProduct(IFormCollection collection)
        {
            try
            {
                string type = collection["cboProdType"];

                if (type.Equals("all"))
                {
                    return View("Product", UserController.dbOp.allProducts(DBoperations.myuserid4product));
                }
                else
                {
                    return View("Product", UserController.dbOp.filteredList(DBoperations.myuserid4product, type));
                }
                
                
            }
            catch
            {
                return View("FarmerProduct");
            }
        }
            

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View(UserController.dbOp.getProductDetails(id));
        }

        // GET: ProductController/Create
        public ActionResult Create(int id)
        {
            
            
            return View(UserController.dbOp.getProductDetails(id));
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id,IFormCollection collection)
        {
            try
            {
                

                //obtain user input from form
               
                string name = collection["lblProductName"];
                string type = collection["cboProdType"];
                string description = collection["lblDescription"];
                double price = Convert.ToDouble( collection["lblPrice"]);
                DateTime dateSupplied = Convert.ToDateTime(collection["lblSupplyDate"]);
                int userID = Convert.ToInt32(collection["lblUserID"]);//UserController.dbOp.UserID;

                //using input create User object that will be sent as a paramter to add a student to the db
                Product p = new Product(name, type, description, price, dateSupplied,userID);

                UserController.dbOp.addProduct(p);
                return RedirectToAction(nameof(Product));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            Product p = UserController.dbOp.getProductDetails(id);
            return View(p);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                UserController.dbOp.editProduct(id, collection["lblName"], collection["cboProdType"], collection["lblDescription"], Convert.ToDouble(collection["lblPrice"]), Convert.ToDateTime(collection["lblSupplyDate"]));
                //UserController.dbOp.editProduct(id, collection[])
                return RedirectToAction(nameof(Product));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            Product p = UserController.dbOp.getProductDetails(id);
            return View(p);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                UserController.dbOp.deleteProduct(id);
                return RedirectToAction(nameof(Product));
            }
            catch
            {
                return View();
            }
        }
    }
}
