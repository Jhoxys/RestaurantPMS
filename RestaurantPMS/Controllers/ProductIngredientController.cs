using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantPMS.Models;
using RestaurantPMS.Service;
using System;

namespace RestaurantPMS.Controllers
{
    public class ProductIngredientController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        public ProductIngredientController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }


        public async Task<IActionResult> Index()
        {
            var products = await context.RawProducts.OrderByDescending(x => x.Id).ToListAsync();
            return View(products);
        
        }

      [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            if (productDto.ImagenFileName == null)
			{
				return RedirectToAction("ggdgd", "Products");
				//ModelState.AddModelError("ImagenFileName", "The Image File is required.");
            }

            if (!ModelState.IsValid)
            {
             
                return View(productDto);
            }

            string newFilename = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFilename += Path.GetExtension(productDto.ImagenFileName!.FileName);

            string imageFullPath = environment.WebRootPath + "/ProductIngredient/" + newFilename;

            using (var stream = System.IO.File.Create(imageFullPath))
            {
                await productDto.ImagenFileName.CopyToAsync(stream);
            }

            RawProduct product = new() // Sintaxis simplificada 'new()'
            {
                Name = productDto.Name,
                Brand = productDto.Brand,
                Category = productDto.Category,
                Price = productDto.Price,
                Description = productDto.Description,
                ImagenFileName = newFilename,
                CreatedAt = DateTime.Now
            };

            context.RawProducts.Add(product);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Products");
        }


        // GET: Products/Edit/{id}
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = context.RawProducts.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            var productDto = new ProductDto()
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                Description = product.Description,
            };

            ViewData["productId"] = product.Id;
            ViewData["ImagenFileName"] = product.ImagenFileName;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

            return View(productDto);
        }

        // POST: Products/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductDto productDto)
        {
            var product = context.RawProducts.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            string oldImagenFileName = product.ImagenFileName;

            if (!ModelState.IsValid)
            {
                ViewData["productId"] = product.Id;
                ViewData["ImagenFileName"] = oldImagenFileName;
                ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");
                return View(productDto);
            }

            string newFilename = oldImagenFileName;
            if (productDto.ImagenFileName != null)
            {
                newFilename = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFilename += Path.GetExtension(productDto.ImagenFileName.FileName);

                string imageFullPath = environment.WebRootPath + "/products/" + newFilename;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    await productDto.ImagenFileName.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(oldImagenFileName) && oldImagenFileName != newFilename)
                {
                    string oldImageFullPath = environment.WebRootPath + "/products/" + oldImagenFileName;
                    if (System.IO.File.Exists(oldImageFullPath))
                    {
                        System.IO.File.Delete(oldImageFullPath);
                    }
                }
            }

            product.Name = productDto.Name;
            product.Brand = productDto.Brand;
            product.Category = productDto.Category;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.ImagenFileName = newFilename;

            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Products");
        }
        
        // GET para la vista de confirmación de eliminación (opcional, pero buena práctica)
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = context.RawProducts.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product); // Retorna la vista de confirmación de eliminación
        }

        // POST para ejecutar la eliminación
        [HttpPost, ActionName("Delete")] // ActionName para que este POST sea llamado por un botón de "Delete"
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await context.RawProducts.FindAsync(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            string imageFullPath = environment.WebRootPath + "/products/" + product.ImagenFileName;
            if (System.IO.File.Exists(imageFullPath))
            {
                System.IO.File.Delete(imageFullPath);
            }

            context.RawProducts.Remove(product);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Products");
        }
        
        // --- Métodos para Ingredientes ---
        [HttpGet]
        public async Task<IActionResult> ManageIngredients(int id)
        {
            var result = new IngredientInProductsDTO();

            var product = await context.RawProducts
                .Include(p => p.ProductIngredients!)
                .ThenInclude(pi => pi.Ingredient)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var allIngredients = await context.Ingredients.OrderBy(i => i.Name).ToListAsync();

            result.productos = product;
            result.ingredients = allIngredients.ToList();


            return View(result);
        }

        [HttpGet]
        public async Task<JsonResult> ManageIngredientsJson(int id)
        {
            var result = new IngredientInProductsDTO();

            var product = await context.RawProducts
                .Include(p => p.ProductIngredients!)
                .ThenInclude(pi => pi.Ingredient)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Producto no encontrado."
                });
            }

            var allIngredients = await context.Ingredients
				.OrderBy(i => i.Name)
                .ToListAsync();

            result.productos = product;
            result.ingredients = allIngredients;

            var serialization = Newtonsoft.Json.JsonConvert.SerializeObject(result, new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Json(new
            {
                success = true,
                data = serialization
            });
        }


        [HttpPost]
        public async Task<IActionResult> AddIngredientToProduct(int productId, int ingredientId, decimal quantity, string unitOfMeasureUsed)
        {
            var product = await context.RawProducts.FindAsync(productId);
            var ingredient = await context.Ingredients.FindAsync(ingredientId);

            if (product == null || ingredient == null)
            {
                return NotFound();
            }

            var existingPi = await context.ProductIngredients
                                .FirstOrDefaultAsync(pi => pi.ProductId == productId && pi.IngredientId == ingredientId);
            if (existingPi != null)
            {
                existingPi.Quantity = quantity;
                existingPi.UnitOfMeasureUsed = unitOfMeasureUsed;
            }
            else
            {
                var productIngredient = new ProductIngredient
                {
                    ProductId = productId,
                    IngredientId = ingredientId,
                    Quantity = quantity,
                    UnitOfMeasureUsed = unitOfMeasureUsed
                };
                context.ProductIngredients.Add(productIngredient);
            }

            await context.SaveChangesAsync();

            return RedirectToAction("ManageIngredients", new { id = productId });
        }


        [HttpPost]
        public async Task<IActionResult> RemoveIngredientFromProduct(int productId, int ingredientId)
        {
            var productIngredient = await context.ProductIngredients
                                                .FirstOrDefaultAsync(pi => pi.ProductId == productId && pi.IngredientId == ingredientId);

            if (productIngredient == null)
            {
                return NotFound();
            }

            context.ProductIngredients.Remove(productIngredient);
            await context.SaveChangesAsync();

            return RedirectToAction("ManageIngredients", new { id = productId });
        }

        /// <summary>
        /// Ejecuta el procedimiento almacenado para insertar un movimiento de producto
        /// y actualizar el inventario.
        /// </summary>
        /// 

        public IActionResult ExecuteMovInvebtary(int id = 3)
        {
            var product = context.RawProducts.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            var productDto = new ProductDto()
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                Description = product.Description,
            };

            ViewData["productId"] = product.Id;
            ViewData["ImagenFileName"] = product.ImagenFileName;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

            return View(productDto);
        }


        public async Task<JsonResult> EjecutarMovimientoProductoAsync(int idProduct, int cantidad, string usuarioCrea, string concepto)
        {
            try
            {
                // Validación simple del concepto
                if (concepto != "Incrementar" && concepto != "Disminuir")
                    throw new ArgumentException("El concepto debe ser 'Incrementar' o 'Disminuir'.");

                // Ejecutar el procedimiento almacenado usando parámetros seguros
                await context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_InsertarMovimientoProducto @IdProduct = {0}, @Cantidad = {1}, @UsuarioCrea = {2}, @Concepto = {3}",
                    idProduct, cantidad, usuarioCrea, concepto
                );
                return Json(new
                {
                    messager = "Ok",
                    success = true
                });
            }
            catch (Exception )
            {

                return Json(new
                {
                    messager = "failid",
                    success = false
                });
            }
        }



        




    }
}
