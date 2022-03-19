using API_Orders;
using API_Orders.Utils;
using API_Orders.Utils.Tree;
using Microsoft.AspNetCore.Mvc;
using Shared_Orders.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Orders.Controllers.api
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ApiController
    {
        //GET:  api/v1/Produits/Gets
        [HttpGet]
        [Route("Gets")]
        public ActionResult<IEnumerable<ProductDTO>> Get()
        {
            try
            {
                IEnumerable<ProductDTO> lProduits = Product.GetAll();
                return Ok(lProduits);
            } catch(Exception ex)
            {
                Log.logger.Error($"[ProductsController - Gets()] Error while calling service : {ex}");
                return Forbid(ex.Message);
            }
        }

        // GET  api/v1/Produits/Get
        [HttpGet]
        [Route("Get")]
        public ActionResult<ProductDTO> Get(int id)
        {
            if (id <= 0) return NotFound();
            try
            {
                ProductDTO produit = Product.Get(id);
                return Ok(produit);
            } catch(Exception ex)
            {
                Log.logger.Error($"[ProductsController - Get(id={id})] Error while calling service : {ex}");
                return Forbid(ex.Message);
            }
            
        }

        //GET:  api/v1/Produits/AsTree
        [HttpGet]
        [Route("AsTree")]
        public ActionResult<IEnumerable<TreeProduit>> AsTree()
        {
            try
            {
                var treeProduits = Product.AsTree();
                return Ok(treeProduits);
            }
            catch (Exception ex)
            {
                Log.logger.Error($"[ProductsController - AsTree()] Error while calling service : {ex}");
                return Forbid(ex.Message);
            }
        }

        //POST  api/v1/Produits/Save
        [HttpPost]
        [Route("Save")]
        public ApiResult Post([FromBody] ProductDTO model)
        {
            try
            {
                if (model == null) return ApiResult.Error(modelEmpty);
                return serviceCall(_services.Produits.Save(Product.ToBusinessObject(model)).statut);
            } catch(Exception ex)
            {
                Log.logger.Error($"[ProductsController - Post(model={model}] Error while calling service : {ex}");
                return ApiResult.Error(internalError);
            }
            
        }

        //DELETE api/v1/Produits/Delete
        [HttpDelete]
        [Route("Delete")]
        public ApiResult Delete(int id)
        {
            try
            {
                if (id <= 0) return ApiResult.Error(modelEmpty);

                var produit = _services.Produits.Get(id).data;
                if (produit == null) return ApiResult.Error(modelEmpty);

                return serviceCall(_services.Produits.Delete(produit).statut);
            } catch(Exception ex) {
                Log.logger.Error($"[ProductsController - Delete(id={id}] Error while calling service : {ex}");
                return ApiResult.Error(internalError);
            }
        }

        #region Catégorie de produits
        //POST  api/v1/Produits/SaveCat
        [HttpPost]
        [Route("SaveCat")]
        public ApiResult PostCat([FromBody]ProductCategoryDTO model)
        {
            try
            {
                if (model == null) return ApiResult.Error(modelEmpty);
                return serviceCall(_services.Produits.SaveCat(CategoryProduct.ToBusinessObject(model)).statut);
            }
            catch (Exception ex)
            {
                Log.logger.Error($"[ProductsController - PostCat(model={model}] Error while calling service : {ex}");
                return ApiResult.Error(internalError);
            }

        }

        //DELETE api/v1/Produits/Delete
        [HttpDelete]
        [Route("DeleteCat")]
        public ApiResult DeleteCat(int id)
        {
            try
            {
                if (id <= 0) return ApiResult.Error(modelEmpty);

                var cat = _services.Produits.GetCat(id).data;
                if (cat == null) return ApiResult.Error(modelEmpty);

                return serviceCall(_services.Produits.DeleteCat(cat).statut);
            }
            catch (Exception ex)
            {
                Log.logger.Error($"[ProductsController - Delete(id={id}] Error while calling service : {ex}");
                return ApiResult.Error(internalError);
            }
        }
        #endregion

        #region Part de produits
        //GET:  api/v1/Produits/GetAllPart
        [HttpGet]
        [Route("GetAllPart")]
        public ActionResult<IEnumerable<PortionDTO>> GetAllPart()
        {
            try
            {
                IEnumerable<PortionDTO> lPart = Portion.GetAll();
                return Ok(lPart);
            }
            catch (Exception ex)
            {
                Log.logger.Error($"[ProductsController - GetAllPart()] Error while calling service : {ex}");
                return Forbid(ex.Message);
            }

        }

        // GET  api/v1/Produits/GetPart
        [HttpGet]
        [Route("GetPart")]
        public ActionResult<ProductDTO> GetPart(int id)
        {
            if (id <= 0) return null;
            try
            {
                PortionDTO part = Portion.Get(id);
                return Ok(part);
            }
            catch (Exception ex)
            {
                Log.logger.Error($"[ProductsController - GetPart(id={id})] Error while calling service : {ex}");
                return Forbid(ex.Message);
            }

        }

        //POST  api/v1/Produits/SavePart
        [HttpPost]
        [Route("SavePart")]
        public ApiResult SavePart([FromBody]PortionDTO model)
        {
            try
            {
                if (model == null) return ApiResult.Error(modelEmpty);
                return serviceCall(_services.Produits.SavePart(Portion.ToBusinessObject(model)).statut);
            }
            catch (Exception ex)
            {
                Log.logger.Error($"[ProductsController - SavePart(model={model}] Error while calling service : {ex}");
                return ApiResult.Error(internalError);
            }

        }

        //DELETE api/v1/Produits/DeletePart
        [HttpDelete]
        [Route("DeletePart")]
        public ApiResult DeletePart(int id)
        {
            try
            {
                if (id <= 0) return ApiResult.Error(modelEmpty);

                var part = _services.Produits.GetPart(id).data;
                if (part == null) return ApiResult.Error(modelEmpty);

                return serviceCall(_services.Produits.DeletePart(part).statut);
            }
            catch (Exception ex)
            {
                Log.logger.Error($"[ProductsController - DeletePart(id={id}] Error while calling service : {ex}");
                return ApiResult.Error(internalError);
            }
        }
        #endregion
    }
}
