using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
namespace proj8.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private readonly ImageService _imageService;

    public List<Uri>? Images {get;set;}

    [Required]
    [Display(Name="Image File")]
    public IFormFile? ImageFile {get;set;}

    public IndexModel(ILogger<IndexModel> logger, ImageService imageService)
    {
        _logger = logger;
        _imageService = imageService;
    }

    public async Task OnGet()
    {
        System.Console.WriteLine("OnGet start");
        Images = await _imageService.ListAsync();
        
        System.Console.WriteLine("OnGet end");
    }

    public async Task<IActionResult> OnPostUploadAsync() {
        if (ImageFile is not null) {
            await _imageService.UploadAsync(ImageFile.FileName, ImageFile.OpenReadStream());
        }
        return RedirectToAction("Get");
    }
}
