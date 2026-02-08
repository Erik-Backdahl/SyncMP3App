using System.Threading.Tasks;

public interface IFolderPickerService
{
    Task<string?> PickFolderAsync();
}