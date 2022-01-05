using AutoMapper;
using Commander.Data;
using Commander.DTOs;
using Commander.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

// Decoupling: This controller accesses the repository's implemented class (SqlCommanderRepo). SqlCommanderRepo accesses the database.

namespace Commander.Controllers
{
    [Route("api/[controller]")] // Route: api/commands
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        // Constructor: dependency is injected into "repository" and "mapper" variables (Dependency injection)
        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //private readonly MockCommanderRepo _repository = new MockCommanderRepo();


        // GET -> api/commands
        [HttpGet]
        [SwaggerOperation(Summary = "Get all commands.")]
        public ActionResult <IEnumerable<CommandReadDTO>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();

            // Return HTTP 200 OK success result + commandItems
            return Ok(_mapper.Map<IEnumerable<CommandReadDTO>>(commandItems));
        }

        // GET -> api/commands/{id}
        [HttpGet("{id}", Name="GetCommandById")]
        [SwaggerOperation(Summary = "Get a single command given the id number.")]
        public ActionResult <CommandReadDTO> GetCommandById(int id)
        {
            var commandItems = _repository.GetCommandById(id);

            if (commandItems != null)
            {
                // Return HTTP 200 OK success result + commandItems
                return Ok(_mapper.Map<CommandReadDTO>(commandItems)); // Map commandItems to CommandReadDTO
            }

            return NotFound();
        }

        
        // POST -> api/commands
        [HttpPost]
        [SwaggerOperation(Summary = "Create a command with three attributes: 'howTo', 'line', and 'platform'.")]
        public ActionResult <CommandReadDTO> CreateCommand(CommandCreateDTO commandCreateDTO)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDTO); // Map commandCreateDTO to Command

            _repository.CreateCommand(commandModel); // Create the model in DB Context
            _repository.SaveChanges();

            var commandReadDTO = _mapper.Map<CommandReadDTO>(commandModel); // Map commandModel to CommandReadDTO

            // Return HTTP 201 + send back URI (REST principle)
            return CreatedAtRoute(nameof(GetCommandById), new {Id = commandReadDTO.Id }, commandReadDTO);

            //return Ok(commandReadDTO);
        }

        // PUT -> api/commands/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update a command; with three attributes: 'howTo', 'line', and 'platform'.")]
        public ActionResult UpdateCommand(int id, CommandUpdateDTO commandUpdateDTO)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);

            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(commandUpdateDTO, commandModelFromRepo);

            _repository.UpdateCommand(commandModelFromRepo);

            _repository.SaveChanges();

            return NoContent(); // return status 204
        }

        // PATCH -> api/commands/{id}
        [HttpPatch("{id}")]
        [SwaggerOperation(Summary = "Update a command; with three attributes: 'op', 'path', and 'value'.")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDTO> patchDoc)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);

            if (commandModelFromRepo == null)
            {
                return NotFound(); // return status 404
            }

            var commandToPatch = _mapper.Map<CommandUpdateDTO>(commandModelFromRepo);

            patchDoc.ApplyTo(commandToPatch, ModelState);

            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandModelFromRepo);

            _repository.UpdateCommand(commandModelFromRepo);

            _repository.SaveChanges();

            return NoContent(); // return status 204
        }

        // DELETE -> api/commands/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete the command associated to the given id number.")]
        public ActionResult CommandDelete(int id)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);

            if (commandModelFromRepo == null)
            {
                return NotFound(); // return status 404
            }

            _repository.DeleteCommand(commandModelFromRepo);

            _repository.SaveChanges();

            return NoContent(); // return status 204
        }

    }
}