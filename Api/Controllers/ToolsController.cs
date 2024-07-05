using System.ComponentModel.DataAnnotations;
using MichaelKjellander.Api.Features.Sudoku;
using MichaelKjellander.Domains.ApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace MichaelKjellander.Api.Controllers;

[ApiController]
[Route("api/tools")]
public class ToolsController  : Controller
{
    [HttpPost]
    [Route("sudoku/solve")]
    public IActionResult SolveSudoku([FromBody] SolveRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        SudokuBoard board = SudokuBoard.CreateBoardFromValues(request.Values!);
        board.Solve();
        
        return Ok(ApiResponseFactory.CreateSimpleApiResponse(board.Values));
    }
    
    public class SolveRequest
    {
        [Required]
        [MinLength(SudokuBoard.TotalNumSquares)]
        [MaxLength(SudokuBoard.TotalNumSquares)]
        public int[]? Values { get; set; }
    }
}