using CentersAPI.Models.EFModels;
using CentersAPI.Models.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Security;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace CentersAPI.Helpers
{
    public class Utilities
    {
        private Entities db = new Entities();

        /* public string RsaDecryptWithPrivate(string base64Input, string privateKey)
         {
             var bytesToDecrypt = Convert.FromBase64String(base64Input);

             AsymmetricCipherKeyPair keyPair;
             var decryptEngine = new Pkcs1Encoding(new RsaEngine());

             using (var txtreader = new StringReader(privateKey))
             {
                 keyPair = (AsymmetricCipherKeyPair)new PemReader(txtreader).ReadObject();

                 decryptEngine.Init(false, keyPair.Private);
             }

             var decrypted = Encoding.UTF8.GetString(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length));
             return decrypted;
         }

         public string RsaEncryptWithPublic(string clearText, string publicKey)
         {
             var bytesToEncrypt = Encoding.UTF8.GetBytes(clearText);

             var encryptEngine = new Pkcs1Encoding(new RsaEngine());

             using (var txtreader = new StringReader(publicKey))
             {
                 var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

                 encryptEngine.Init(true, keyParameter);
             }

             var encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));
             return encrypted;

         }*/
        public bool IsAuthorizedUser(string Username, string Password)
        {
            return db.Channels.Any(Channel => Channel.ChannelId == Username && Channel.Password == Password);//Username == "bhushan" && Password == "demo";
        }
        //=================================================================================================
        //=== GET Error Messages :
        //=================================================================================================
        public static ErrorModel GetErrorMessages(string responseCode)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();
            ErrorModel errorItem = new ErrorModel();

            if (responseCode == null) responseCode = "-1";

            try
            {
                XmlRootAttribute xRoot = new XmlRootAttribute();
                xRoot.ElementName = "ErrorList";
                xRoot.IsNullable = true;

                TextReader txtReader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Messages.xml"));
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ErrorModel>), xRoot);
                errorList = (List<ErrorModel>)xmlSerializer.Deserialize(txtReader);
                txtReader.Close();

                errorItem = errorList.Find(x => x.ErrorCode == responseCode);
            }
            catch (Exception)
            {
                if (responseCode == "0")
                {
                    errorItem.ErrorCode = "0";
                    errorItem.ErrorMessage = "Successful";
                    errorItem.ErrorMessageAr = "تمت العملية بنجاح";
                }
                else
                {
                    errorItem.ErrorCode = "-2";
                    errorItem.ErrorMessage = "An error occured";
                    errorItem.ErrorMessageAr = "حدث خطأ ما";
                }
            }

            return errorItem;
        }   
        public static bool sendMail(string VerifyCode, string Email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("qualifyapi@gmail.com");
                mail.To.Add(Email);
                mail.Subject = "Qualify";
                mail.Body = "Your Verify Code is  : "+ VerifyCode;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("qualifyapi@gmail.com", "Aishame_911");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public  Double GetCenterRate(int centerId)
        {
            var centerRates = db.CenterRates.Where(c => c.CenterId == centerId).ToList();
            int count = centerRates.Count;
            int sum = centerRates.Sum(c => c.Rate);
            if (count != 0)
                return sum / count;
            else return 0;
        }
    }
}