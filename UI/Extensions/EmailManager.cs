using SendGrid;
using SendGrid.Helpers.Mail;

namespace UI.Extensions
{
    public class EmailManager
    {
        public static void SendEmail(string To, string body, string Subject)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("notifications@plusb.com", "PlusB Service Desk");
            var subject = Subject;
            var to = new EmailAddress(To, "Customer");
            var plainTextContent = body;
            var htmlContent = "<!doctype html> " +
                                 "<html>" +
                                   "<body style='background-color:#85A885;font-family:sans-serif;font-size:14px;-webkit-font-smoothing:antialiased;line-height:1.4;margin:0;padding:0;-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%;'>" +
                                     "<br/><br/><table border='0' cellpadding='0'cellspacing='0' style='border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%;'>" +
                                      " <tr>" +
                                         "<td>&nbsp;</td>" +
                                         "<td style='display:block;margin:0 auto !important;max-width:580px;padding:10px;width:580px;'>" +
                                           "<div style='box-sizing:border-box;display: block;margin:0 auto;max-width:580px;padding:10px;'>" +
                                             "<table style='background:#ffffff;border-radius:3px;width:100%;border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%;'>" +
                                               "<tr>" +
                                                 "<td style='box-sizing:border-box;padding:20px;'>" +
                                                   "<table border='0' cellpadding='0'cellspacing='0' style='border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%;'>" +
                                                     "<tr>" +
                                                       "<td style='font-family:sans-serif;font-size:14px;vertical-align:top;'>" +
                                                         "<p>Hi there,</p>" +
                                                         "<p>"+ body +" </p>" +
                                                         "<table border='0' cellpadding='0'cellspacing='0'>" +
                                                           "<tbody>" +
                                                             "<tr>" +
                                                               "<td align='left'>" +
                                                                 "<table border='0' cellpadding='0'cellspacing='0'>" +
                                                                "   <tbody>" +
                                                               "      <tr>" +
                                                             "        </tr>" +
                                                            "       </tbody>" +
                                                           "      </table>" +
                                                          "     </td>" +
                                                         "    </tr>" +
                                                        "   </tbody>" +
                                                       "  </table>" +
                                                      "   <br/>" +
                                                      "   <p style='font-size:12px;'>This is an automatic email sent by the PlusB Consulting Service Desk system.</p>" +
                                                     "    <p style='font-size:12px;'>Thanks for being part of our business!</p>" +
                                                    "   </td>" +
                                                   "  </tr>" +
                                                 "  </table>" +
                                               "  </td>" +
                                              " </tr>" +
                                             "</table>" +
                                             "<br/>" +
                                             "<div style='clear:both;margin-top:10px;width:100%;'>" +
                                               "<table border='0' cellpadding='0' cellspacing='0' style='border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%;'>" +
                                                 "<tr><br/>" +
                                                   "<td style='padding-bottom:10px;padding-top:10px;color:#000;font-size:12px;'>" +
                                                     "<span style='color:#000;font-size:12px;'>PlusB Consulting Escazú, San José Costa Rica</span>" +
                                                    " <br/> Don't like these emails? <a style='color:#000;font-size:12px;'>Unsubscribe</a>." +
                                                  " </td>" +
                                                 "</tr>" +
                                                 "<tr>" +
                                                   "<td style='padding-bottom:10px;padding-top:10px;text-decoration:none;color:#000;font-size:12px;'>" +
                                                     "Powered by <a style='color:#000;font-size:12px;'>PlusB Consulting S.A</a>." +
                                                   "</td>" +
                                                 "</tr>" +
                                               "</table>" +
                                             "</div" +
                                           "</div>" +
                                         "</td>" +
                                         "<td style='color:#000;font-size:12px;'>&nbsp;</td>" +
                                       "</tr>" +
                                     "</table>" +
                                  "</body>" +
                                 "</html>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }
    }
}