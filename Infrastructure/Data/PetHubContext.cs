using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class PetHubContext(DbContextOptions options) : DbContext(options)
{
    
}
