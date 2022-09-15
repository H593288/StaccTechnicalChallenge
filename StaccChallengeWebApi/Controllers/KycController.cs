// https://flurl.dev/ easier to do HTTP request
using Flurl.Http;
// webframe work for .netcore. use mvc library 
using Microsoft.AspNetCore.Mvc;

// package - group classes to organise
namespace StaccChallengeWebApi.Controllers;

// attribute - add meta data to a function. 
[ApiController]
[Route("[controller]/[action]")]
// inheriting controller
public class KycController : Controller
{
    // GET, retrieves data based on url 
    [HttpGet]
    // function declaration. async > way of doing threads. Asynchronous function must return task. 
    // 3 api endpoints , individual name find matches in a pep database and return it, second organisation number return all the
    // important i people in the company, takes a organisnr but returning basic company info
    
    public async Task<IActionResult> PepSearch(string name)
    {
        // build an url request and get response as json 
        // task is a thread, asynchronous function
        // await task, get the type out which is dynamic
        // putting name string inside another string > interpolated 
        // extention method from flurl library
        var searchResult = await $"https://code-challenge.stacc.dev/api/pep?name={name}".GetJsonAsync();
        
        // ok response 200 HTTP response
        return Ok(searchResult);
    }

    [HttpGet]
    public async Task<IActionResult> CompanyRolesSearch(string orgNumber)
    {
        var searchResult = await $"https://code-challenge.stacc.dev/api/roller?orgNr={orgNumber}".GetJsonListAsync();

        return Ok(searchResult);
    }

    [HttpGet]
    public async Task<IActionResult> CompanyInfoSearch(string orgNumber)
    {
        var searchResult = await $"https://code-challenge.stacc.dev/api/enheter?orgNr={orgNumber}".GetJsonAsync();

        return Ok(searchResult);
    }
}