﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace SystemCommonLibrary.Network
{
	public class Mail
	{
		/// <summary>
		/// 发送邮件
		/// </summary>
		public static void SendMail (string body, string toMail, string subject, string fromMail, string smtp, string pwd)
		{
			MailMessage msg = new MailMessage ();
			msg.To.Add(toMail);//收件人邮箱
			msg.Subject = subject;//邮件标题
			msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码格式
			msg.From = new MailAddress (fromMail);//发件人邮箱
			msg.IsBodyHtml = true;//内容是html格式
			msg.Body = body;//内容
			msg.BodyEncoding = System.Text.Encoding.UTF8;//内容编码格式
			msg.Priority = MailPriority.Normal;//优先级
			SmtpClient client = new SmtpClient ();
			client.Host = smtp;//邮箱服务器
			client.Credentials = new System.Net.NetworkCredential (fromMail, pwd);//帐号密码
			client.Send (msg);
		}

		/// <summary>
		/// 发送包含附件的邮件
		/// </summary>
		public static void SendMail(string body, string toMail, string subject, string fromMail, string smtp, string pwd, string attachmentPath)
		{
			MailMessage msg = new MailMessage();
			msg.To.Add(toMail);//收件人邮箱
			msg.Subject = subject;//邮件标题
			msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码格式
			msg.From = new MailAddress(fromMail);//发件人邮箱
			msg.IsBodyHtml = true;//内容是html格式
			msg.Body = body;//内容
			msg.BodyEncoding = System.Text.Encoding.UTF8;//内容编码格式
			msg.Priority = MailPriority.Normal;//优先级
			msg.Attachments.Add(new Attachment(attachmentPath));
			SmtpClient client = new SmtpClient();
			client.Host = smtp;//邮箱服务器
			client.Credentials = new System.Net.NetworkCredential(fromMail, pwd);//帐号密码
			client.Send(msg);
		}
	}
}
