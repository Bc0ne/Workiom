namespace Workiom.Core
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Entities.Core;

    public class DomainEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public DateTime? DeletionDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
