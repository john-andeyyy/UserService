using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Manager;
using UserService.Models.Manager;
using UserService.Models.Request;

namespace UserService.Controllers
{
    [Route("api/TodoService/")]
    [ApiController]
    [Authorize(Policy = "AuthPolicy")]

    public class Todo_ServiceController : Controller
    {

        #region Using params
        [HttpGet("UserTodoById/{UserId}")]
        public IActionResult Get([FromRoute] int UserId)
        {
            ServiceManager serviceManager = new ServiceManager();

            var service = serviceManager.GetServiceMethodDetails("TodoService", "GetUserTodos");
            if (service == null)
            {
                return NotFound("Invalid service request");
            }
            var response = serviceManager.ApiUsingParams(service, UserId);


            return Content(response, "application/json");


        }

        [HttpDelete("Delete/{Id}")]
        public IActionResult Delete([FromRoute] int Id)
        {
            ServiceManager serviceManager = new ServiceManager();

            var service = serviceManager.GetServiceMethodDetails("TodoService", "DeleteTodo");
            if (service == null)
            {
                return NotFound("Invalid service request");
            }
            var response = serviceManager.ApiUsingParams(service, Id);


            return Content(response, "application/json");
        }

        [HttpGet("TodoID/{Id}")]
        public IActionResult GetTodoById([FromRoute] int Id)
        {
            ServiceManager serviceManager = new ServiceManager();

            var service = serviceManager.GetServiceMethodDetails("TodoService", "GetTodoById");
            if (service == null)
            {
                return NotFound("Invalid service request");
            }
            var response = serviceManager.ApiUsingParams(service, Id);


            return Content(response, "application/json");
        }

        #endregion


        [HttpPost("NewTodo")]
        public IActionResult NewTodo(NewTodo newTodo)
        {
            var manager = new UserManager();
            var IsExcisting = manager.GetUserById(newTodo.UserId);
            if (IsExcisting == null)
            {
                return NotFound(new { Message = "User Not Found! " });
            }

            ServiceManager serviceManager = new ServiceManager();
            var service = serviceManager.GetServiceMethodDetails("TodoService", "NewTodo");
            if (service == null)
            {
                return NotFound("Invalid service request");
            }
            var response = serviceManager.SendDynamicApi(service, newTodo);

            return Content(response, "application/json");
        }

        [HttpPut("Update")]
        public IActionResult Update(UpdateTodo newTodo)
        {
            ServiceManager Manager = new ServiceManager();
            var service = Manager.GetServiceMethodDetails("TodoService", "UpdateTodo");
            if (service == null)
            {
                return NotFound("Invalid service request");
            }

            var Response = Manager.SendDynamicApi(service, newTodo);
            return Content(Response, "application/json");

        }

        [HttpPut("IsCompleted")]
        public IActionResult IsCompleted([FromBody] UpdateTodo Update)
        {
            ServiceManager serviceManager = new ServiceManager();

            var service = serviceManager.GetServiceMethodDetails("TodoService", "MarkAsCompleted");
            if (service == null)
            {
                return NotFound("Invalid service request");
            }
            var response = serviceManager.SendDynamicApi(service, Update);

            return Content(response, "application/json");
        }


    }
}
