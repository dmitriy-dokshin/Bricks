#region

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;

using Bricks.Core.Sync;

#endregion

namespace Bricks.DAL.EF
{
	public static class ModelBuilderExtensions
	{
		public static void ConfigreLocks(this DbModelBuilder modelBuilder)
		{
			var configuration = modelBuilder.Entity<Lock>();
			configuration.ToTable("Locks");
			configuration.HasKey(x => x.Id);
			configuration.Property(x => x.Key).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UX_Key") { IsUnique = true }));
		}
	}
}