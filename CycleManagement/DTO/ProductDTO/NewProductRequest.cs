namespace CycleManagement.DTO.ProductDTO
{
    public class NewProductRequest
    {
        public string name;
        public string brand;
        public string modelNumber;
        public Guid categoryId;
        public int price;
        public IFormFile photo;

    }
}
