namespace electronic_equipment_store.Models.MenuModel
{
    public class ListMenu
    {
        public List<ItemMenu> OwnerHeaderMenu = new List<ItemMenu>()
        {
            new ItemMenu("Home","Index","Electronic store"),
            new ItemMenu("Home","Products", "Каталог"),
            new ItemMenu("Home", "Promotions", "Акции"),
            new ItemMenu("Home", "Manufacturers", "Производители"),
            new ItemMenu("Home", "Payments", "Платежи"),
            new ItemMenu("Authentication", "Exit", "Выйти"),           
        };
        public List<ItemMenu> AdminHeaderMenu = new List<ItemMenu>()
        {
            new ItemMenu("Home","Index","Electronic store"),
            new ItemMenu("Home","Products", "Каталог"),
            new ItemMenu("Home", "Promotions", "Акции"),
            new ItemMenu("Home", "Manufacturers", "Производители"),
            new ItemMenu("Authentication", "Exit", "Выйти"),
        };
        public List<ItemMenu> EmployeeHeaderMenu = new List<ItemMenu>()
        {
            new ItemMenu("Home","Index","Electronic store"),
            new ItemMenu("Home","Products", "Каталог"),
            new ItemMenu("Home", "Promotions", "Акции"),
            new ItemMenu("Home", "Manufacturers", "Производители"),
            new ItemMenu("Authentication", "Exit", "Выйти"),
        };
        public List<ItemMenu> GuestHeaderMenu = new List<ItemMenu>()
        {
            new ItemMenu("Home","Index","Electronic store"),
            new ItemMenu("Home","Products", "Каталог"),
            new ItemMenu("Home", "Promotions", "Акции"),
            new ItemMenu("Home", "Manufacturers", "Производители"),
            new ItemMenu("Authentication", "Login", "Войти"),
        };
        public List<ItemMenu> UserHeaderMenu = new List<ItemMenu>()
        {
            new ItemMenu("Home","Index","Electronic store"),
            new ItemMenu("Home","Products", "Каталог"),
            new ItemMenu("Home", "Promotions", "Акции"),
            new ItemMenu("Home", "Manufacturers", "Производители"),
            new ItemMenu("Authentication", "Exit", "Выйти"),
        };
        public List<ItemMenu> GuestFooterMenu = new List<ItemMenu>()
        {
            new ItemMenu("Main","Index","О нас"),
            new ItemMenu("Main", "Reviews", "Отзывы"),
        };
        public List<ItemMenu> EmployeeFooterMenu = new List<ItemMenu>()
        {
            new ItemMenu("Main","Index","О нас"),
            new ItemMenu("Main", "Reviews", "Отзывы"),
        };
        public List<ItemMenu> OwnerFooterMenu = new List<ItemMenu>()
        {
            new ItemMenu("Main","Index","О нас"),
            new ItemMenu("Main","Users", "Пользователи"),
            new ItemMenu("Main", "Reviews", "Отзывы"),
        };
        public List<ItemMenu> AdminFooterMenu = new List<ItemMenu>()
        {
            new ItemMenu("Main","Index","О нас"),
            new ItemMenu("Main","Users", "Пользователи"),
            new ItemMenu("Main", "Reviews", "Отзывы"),
        };
        public List<ItemMenu> UserFooterMenu = new List<ItemMenu>()
        {
            new ItemMenu("Main","Index","О нас"),
            new ItemMenu("Main", "Reviews", "Отзывы"),
            new ItemMenu("Main", "Order", "Заказ"),
        };
    }
}
