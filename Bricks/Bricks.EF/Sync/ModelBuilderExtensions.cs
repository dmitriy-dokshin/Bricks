#region

using System.Data.Entity;
using Bricks.EF.Entities;

#endregion

namespace Bricks.EF.Sync
{
	public static class ModelBuilderExtensions
	{
		public static void ConfigreLocks(this DbModelBuilder modelBuilder)
		{
			var configuration = modelBuilder.Entity<Lock>();
			configuration.ToTable("Locks");
			configuration.HasKey(x => new {x.Key, x.Key1});
			configuration.Property(x => x.Key).HasMaxLength(50);
			configuration.Property(x => x.Key1).HasMaxLength(50);
		}
	}
}