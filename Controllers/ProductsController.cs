using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantPMS.Models;
using RestaurantPMS.Service;

namespace RestaurantPMS.Controllers
{
 //   [Authorize(Roles = "admin")]
    [Route("/Admin/[controller]/{action=Index}/{id?}")] // establecemos produc sin el index por defecto en el patter
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        private readonly int pageSize = 5;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index(int pageIndex, string? search, string? column, string? orderBy)
        {
            IQueryable<Product> query = context.Products;


            // search funtionality
            if (search != null)
            {
                query = query.Where(s => s.Name.Contains(search) );// agregamos una consulta de resultados
            }

            // sort funtionalityu
            string[] valiColums = { "Id", "Name", "Brand", "Category", "Price", "CreatedAt" };
            string[] valiOrderBy = { "desc", "asc" };

            if (!valiColums.Contains(column))
            {
                column = "Id";
            }
            if (!valiOrderBy.Contains(orderBy))
            {
                orderBy = "desc";
            }

            if (column == "Name")
            {

                if (orderBy == "desc")
                {
                    query = query.OrderBy(s => s.Name);// agregamos una consulta de resultados
                }
                else
                {
                    query = query.OrderByDescending(s => s.Name);// agregamos una consulta de resultados
                }

            }

            if (column == "Description")
            {
                if (orderBy == "desc")
                {
                    query = query.OrderBy(s => s.Description);// agregamos una consulta de resultados
                }
                else
                {
                    query = query.OrderByDescending(s => s.Description);// agregamos una consulta de resultados
                }
            }

            if (column == "Category")
            {
                if (orderBy == "desc")
                {
                    query = query.OrderBy(s => s.Category);// agregamos una consulta de resultados
                }
                else
                {
                    query = query.OrderByDescending(s => s.Category);// agregamos una consulta de resultados
                }
            }

            if (column == "Price")
            {
                if (orderBy == "desc")
                {
                    query = query.OrderBy(s => s.UnidPrice);// agregamos una consulta de resultados
                }
                else
                {
                    query = query.OrderByDescending(s => s.UnidPrice);// agregamos una consulta de resultados
                }
            }

            if (column == "CreatedAt")
            {
                if (orderBy == "desc")
                {
                    query = query.OrderBy(s => s.CreatedAt);// agregamos una consulta de resultados
                }
                else
                {
                    query = query.OrderByDescending(s => s.CreatedAt);// agregamos una consulta de resultados
                }
            }

            if (column == "Id")
            {
                if (orderBy == "desc")
                {
                    query = query.OrderBy(s => s.Id);// agregamos una consulta de resultados
                }
                else
                {
                    query = query.OrderByDescending(s => s.Id);// agregamos una consulta de resultados
                }
            }


            // Pagination functionality
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            decimal count = query.Count(); // total del numero de paginas
            int totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var products = query.ToList();
            ViewData["PageIndex"] = pageIndex;
            ViewData["TotalPages"] = totalPages;

            ViewData["Search"] = search ?? "";
            ViewData["Column"] = column ?? "";
            ViewData["OrderBy"] = orderBy ?? "";


            return View(products);
        }

        public IActionResult Create()
        {
            //var products = context.Products.OrderByDescending(s => s.Id).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {

            if (productDto.ImagenFileName == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(productDto);

            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.ImagenFileName!.FileName);// agragar fecha y la extencion de la imagen

            string imageFileFulPath = environment.WebRootPath + "/img/" + newFileName;

            using (var stream = System.IO.File.Create(imageFileFulPath))
            {
                productDto.ImagenFileName.CopyTo(stream);
            }

            // save thje new producto in the database
            Product produts = new Product()
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Category = productDto.Category,
                UnidPrice = productDto.UnidPrice,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
            };

            context.Products.Add(produts); //  inserta
            context.SaveChanges();  // guarda cambio

            return RedirectToAction("Index", "Products");
        }

        public IActionResult Edit(int id)
        {

            var product = context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            var ProductDto = new ProductDto()
            {
                Name = product.Name,
              //  Brand = product.Brand,
                Category = product.Category,
                UnidPrice = product.UnidPrice,
                Description = product.Description
            };

            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreateAt"] = product.CreatedAt.ToString("mm/dd/yyyy");

            return View(ProductDto);
        }


        public IActionResult vers(int id)
        {

           

            return View();
        }


        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
        {

            var product = context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreateAt"] = product.CreatedAt.ToString("mm/dd/yyyy");

                return View(productDto);

            }
            // Udate the image file if we have a new image file

            string newFileName = product.ImageFileName;

            if (productDto.ImagenFileName != null)
            {

                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productDto.ImagenFileName!.FileName);// agragar fecha y la extencion de la imagen

                string imageFileFulPath = environment.WebRootPath + "/img/" + newFileName;

                using (var stream = System.IO.File.Create(imageFileFulPath))
                {
                    productDto.ImagenFileName.CopyTo(stream);
                }

                // delete thne old image

                string olImageFullPath = environment.WebRootPath + "/img/" + product.ImageFileName;
                System.IO.File.Delete(olImageFullPath);

            }

            // update the product in the database

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Category = productDto.Category;
            product.UnidPrice = productDto.UnidPrice;
            product.ImageFileName = newFileName;


            context.SaveChanges();  // guarda cambio

            return RedirectToAction("Index", "Products");

            // return View(product);

        }
        public IActionResult Delete(int id)
        {

            var product = context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            // delete thne old image

            string olImageFullPath = environment.WebRootPath + "/img/" + product.ImageFileName;
            System.IO.File.Delete(olImageFullPath);

            context.Products.Remove(product);// eliminaamos
            context.SaveChanges(true);  // guarda cambio
            return RedirectToAction("Index", "Products");
        }
    }
}
