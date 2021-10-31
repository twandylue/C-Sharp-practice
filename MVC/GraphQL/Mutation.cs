// using System.Threading.Tasks;
// using MVC.GraphQL.Data;
// using MVC.Respository;
// using HotChocolate;
// using System;
// using MVC.GraphQL.Extensions;

// namespace MVC.GraphQL
// {
//     public class Mutation
//     {
//         [UseApplicationDbContext]
//         public async Task<AddSpeakerPayload> AddSpeakerAsync(AddSpeakerInput input, [Service] PostgresDBContext context)
//         {
//             var speaker = new Speaker
//             {
//                 Name = input.Name,
//                 Bio = input.Bio,
//                 Website = input.WebSite
//             };
//             context.Speakers.Add(speaker);
//             await context.SaveChangesAsync();

//             return new AddSpeakerPayload(speaker);
//         }
//     }
// }