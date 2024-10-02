using Dapper;
using Infrastructure.DataModels;
using Npgsql;

namespace Infrastructure.Repositories;

public class ProductRepository
{
    private NpgsqlDataSource _dataSource;
    public ProductRepository(NpgsqlDataSource dataSource){
        _dataSource = dataSource;
    }

    public IEnumerable<Product> GetAllProducts(){
        string sql = $@"SELECT product_id AS {nameof(Product.Id)}, 
                        product_sku AS {nameof(Product.Sku)}, 
                        product_description AS {nameof(Product.Description)}, 
                        product_registration_date AS {nameof(Product.RegistrationDate)}, 
                        product_move_date AS {nameof(Product.MoveDate)}, 
                        product_balance AS {nameof(Product.Balance)} 
                        FROM products
                    ORDER BY product_description";

        using (var conn = _dataSource.OpenConnection()){
            return conn.Query<Product>(sql);
        }
    }

    public bool SkuExists(string sku){
        string sql = @"SELECT COUNT(*)  
                          FROM products
                         WHERE products.product_sku = @sku";

        using (var conn = _dataSource.OpenConnection()){
            return conn.ExecuteScalar<int>(sql, new { sku }) > 0;
        }
    }

    public Product GetProductsBySku(string sku){

        string sql = $@"SELECT product_id AS {nameof(Product.Id)}, 
                        product_sku AS {nameof(Product.Sku)}, 
                        product_description AS {nameof(Product.Description)}, 
                        product_registration_date AS {nameof(Product.RegistrationDate)}, 
                        product_move_date AS {nameof(Product.MoveDate)}, 
                        product_balance AS {nameof(Product.Balance)} 
                        FROM products
                       WHERE products.product_sku = @sku
                    ORDER BY product_description";

        using (var conn = _dataSource.OpenConnection()){
            return conn.QueryFirst<Product>(sql, new { sku = sku });
        }
    }

    public Object CreateProduct(string _sku, string _description)
    {
        var sql = $@"INSERT INTO products (product_sku, 
                                          product_description, 
                                          product_registration_date, 
                                          product_balance) 
                          VALUES (@product_sku, @product_description, 
                                  @product_registration_date, @product_balance);";
        using (var conn = _dataSource.OpenConnection())
        {
            var _balance = 0;
            return conn.Query(sql, new { product_sku = _sku, 
                                         product_description = _description, 
                                         product_registration_date = DateTime.Now, 
                                         product_balance = _balance });
        }
    }

    public void UpdateProduct(Guid product_id, 
                              decimal product_balance, 
                              DateTime product_move_date)
    {
        var sql = $@"UPDATE products
                      SET product_balance = @product_balance,
                          product_move_date = @product_move_date
                      WHERE product_id = @product_id;";
        using (var conn = _dataSource.OpenConnection())
        {
            conn.Execute(sql, new { product_balance = product_balance,
                                    product_move_date = product_move_date, 
                                    product_id = product_id });
        }
    }

    public Product GetProductById(Guid idProduct)
    {
        string sql = $@"SELECT product_id AS {nameof(Product.Id)}, 
                        product_sku AS {nameof(Product.Sku)}, 
                        product_description AS {nameof(Product.Description)}, 
                        product_registration_date AS {nameof(Product.RegistrationDate)}, 
                        product_move_date AS {nameof(Product.MoveDate)}, 
                        product_balance AS {nameof(Product.Balance)} 
                        FROM products
                       WHERE products.product_id = @idProduct
                    ORDER BY product_description";

        using (var conn = _dataSource.OpenConnection()){
            return conn.QueryFirst<Product>(sql, new { idProduct = idProduct });
        }
    }
}