using Dapper;
using Infrastructure.DataModels;
using Npgsql;

namespace Infrastructure.Repositories;

public class MovementRepository
{
    private NpgsqlDataSource _dataSource;
    public MovementRepository(NpgsqlDataSource dataSource){
        _dataSource = dataSource;
    }

    public IEnumerable<Movement> GetMovementsForFeed(){
        string sql = $@"SELECT movement_id AS {nameof(Movement.Id)}, 
                        movement_id_product AS {nameof(Movement.IdProduct)}, 
                        movement_type AS {nameof(Movement.MovementType)}, 
                        movement_date AS {nameof(Movement.MovementDate)}, 
                        movement_quantity AS {nameof(Movement.Quantity)}
                        FROM movements
                    ORDER BY movement_date DESC;";

        using (var conn = _dataSource.OpenConnection()){
            return conn.Query<Movement>(sql);
        }
    }

    public Object CreateMovement(Guid _Id, int _type, 
                                decimal _quantity, 
                                DateTime _movementDate)
    {
        var sql = $@"INSERT INTO movements (movement_id_product, 
                                          movement_type, 
                                          movement_date, 
                                          movement_quantity) 
                          VALUES (@movement_id_product, @movement_type, 
                                  @movement_date, @movement_quantity);";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query(sql, new { movement_id_product = _Id, 
                                         movement_type = _type, 
                                         movement_date = _movementDate, 
                                         movement_quantity = _quantity });
        }
    }

    public Movement? GetLastMovementByProd(Guid movement_id_product)
    {
        string sql = $@"SELECT movement_id AS {nameof(Movement.Id)}, 
                        movement_id_product AS {nameof(Movement.IdProduct)}, 
                        movement_type AS {nameof(Movement.MovementType)}, 
                        MAX(movement_date) AS {nameof(Movement.MovementDate)}, 
                        movement_quantity AS {nameof(Movement.Quantity)}
                        FROM movements
                    WHERE movement_id_product = @movement_id_product
					GROUP BY movement_id, 
                        movement_id_product, 
                        movement_type,  
                        movement_quantity 
                    ORDER BY movement_date DESC;";

        using (var conn = _dataSource.OpenConnection())
        {
            try
            {
                return conn.QueryFirstOrDefault<Movement>(sql, new { movement_id_product = movement_id_product });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while trying to get the last movement for product {movement_id_product}: {ex.Message}");
                return null;
            }
        }
    }

    public void DeleteMovement(Guid movement_id)
    {
        var sql = $@"DELETE FROM movements
                      WHERE movement_id = @movement_id;";
        using (var conn = _dataSource.OpenConnection())
        {
            conn.Execute(sql, new { movement_id = movement_id });
        }
    }
}