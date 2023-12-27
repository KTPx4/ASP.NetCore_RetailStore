using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

public class GmailSender
{
	static string ApplicationName = "Gmail API .NET Quickstart";
	static string ClientId = "931660209578-j0n6pp9tqj32uvo88t0hgljd59cd6of5.apps.googleusercontent.com";
	static string ClientSecret = "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";
	static string RefreshToken = "1//04d7WVRN0L0TVCgYIARAAGAQSNwF-L9Ir9hivKo9xynNopCVxO017cXf7Ilxy87rqCmcIAablioooVO9WKjP9as9CzSySZLxL7wM";

	public async Task SendEmail(string toEmail, string Subject, string Body)
	{
		System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

		var credential = await GetCredentialAsync();

		var service = new GmailService(new BaseClientService.Initializer()
		{
			HttpClientInitializer = credential,
			ApplicationName = ApplicationName,
		});

		var msg = new AE.Net.Mail.MailMessage
		{
			Subject = Subject,
			Body = Body,
			From = new MailAddress("px4.vnd@gmail.com", "Anh Admin Dep Choai"),
			
		};
		msg.ContentType = "text/html";
		msg.To.Add(new MailAddress(toEmail));
		msg.ReplyTo.Add(msg.From);
		var msgStr = new StringWriter();
		msg.Save(msgStr);

		var result = service.Users.Messages.Send(new Google.Apis.Gmail.v1.Data.Message
		{
			Raw = Base64UrlEncode(msgStr.ToString())
		}, "me").Execute();

		Console.WriteLine("Message ID: " + result.Id);
	}

	private async Task<UserCredential> GetCredentialAsync()
	{
		var clientSecrets = new ClientSecrets
		{
			ClientId = ClientId,
			ClientSecret = ClientSecret
		};

		var tokenResponse = await new AuthorizationCodeFlow(
			new GoogleAuthorizationCodeFlow.Initializer
			{
				ClientSecrets = clientSecrets
			})
			.RefreshTokenAsync(ClientId, RefreshToken, new System.Threading.CancellationToken());

		return new UserCredential(new AuthorizationCodeFlow(
			new GoogleAuthorizationCodeFlow.Initializer
			{
				ClientSecrets = clientSecrets
			}),
			"user",
			tokenResponse);
	}

	private static string Base64UrlEncode(string input)
	{
		var inputBytes = Encoding.Default.GetBytes(input);
		return Convert.ToBase64String(inputBytes).Replace('+', '-').Replace('/', '_').Replace("=", "");
	}
}
