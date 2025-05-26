using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

public class SixDigitTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
{
    public SixDigitTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<DataProtectionTokenProviderOptions> options, ILogger<DataProtectorTokenProvider<TUser>> logger)
        : base(dataProtectionProvider, options, logger) { }

    public override async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        // Tạo token gốc
        var token = await base.GenerateAsync(purpose, manager, user);
        // Lấy 6 số cuối từ token gốc (hoặc random 6 số)
        var otp = new Random().Next(100000, 999999).ToString();
        return otp;
    }
}