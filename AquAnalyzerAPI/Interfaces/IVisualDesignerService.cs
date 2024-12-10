namespace AquAnalyzerAPI.Interfaces
{
    public interface IVisualDesignerService
    {
        Task<IEnumerable<VisualDesigner>> GetAllVisDesig();
        Task<VisualDesigner> GetByIdOfVisDesig(int id);
        Task<VisualDesigner> AddVisDesig(VisualDesigner visualDesigner);
        Task<VisualDesigner> UpdateVisDesig(VisualDesigner visualDesigner);
        Task DeleteVisDesig(int id);
    }
}
