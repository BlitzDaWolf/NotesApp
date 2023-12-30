using Database.Context;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ChangeLogService : IChangeLogService
    {
        readonly NotesContext context;

        public ChangeLogService(NotesContext context)
        {
            this.context = context;
        }

        public async Task<string> CreateLog(Guid noteId, string newText)
        {
            using var activity = Instrumentation.GetActivitySource<ChangeLogService>().StartActivity("Creating log");
            string fullHash = "";
            using (SHA256 sha256 = SHA256.Create())
            {
                using var shaActivity = Instrumentation.GetActivitySource<ChangeLogService>().StartActivity("Creating hash");
                Encoding encoding = Encoding.UTF8;
                var hash = sha256.ComputeHash(encoding.GetBytes(newText));
                fullHash = encoding.GetString(hash);
            }
            await context.ChangeLogs.AddAsync(new Database.Entities.ChangeLog { Hash = fullHash, NoteId = noteId });
            return fullHash;
        }
    }
}
