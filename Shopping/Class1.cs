using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using System.Xml.Linq;

using Newtonsoft.Json;
using RestSharp;

namespace AlignShopping
{

    
   public class ProductResponseval
    {
       public string row_id { get; set; }
       public string product_id { get; set; }
       public string merchant_id { get; set; }
       public string product_name { get; set; }
       public string product_description { get; set; }
       public string product_price { get; set; }
       public string product_shipping { get; set; }
       public string product_sku { get; set; }
       public string date_added { get; set; }
    }
   public class ProductRecord
   {
     public  string status { get; set; }
     public string error { get; set; }
     public string total_count { get; set; }
     public ProductRecord record;       
   }
    #region Product Class Inherting Token System Class
    public class AlignAPI
    {
        //grant_type string Required. Default value for merchant is client_credentials 
        //client_id string Required. The client ID you received from Align Commerce when you registered. 
        //client_secret string Required. The client secret you received from Align Commerce when you registered. 
        //scope string A comma separated list of scope. (products, buyer, invoice) 
        //state string An unguessable random string. It is used to protect against cross-site request forgery attacks.    
        protected string access_token { get; set; }
        protected string token_type { get; set; }
        protected string expires { get; set; }
        protected string expires_in { get; set; }
        protected string refresh_token { get; set; }
        protected string user { get; set; }
        protected string pass { get; set; }
        protected string client_id { get; set; }
        protected string client_secret { get; set; }
        protected string scope { get; set; }
        protected string state { get; set; }
        protected string credentials { get; set; }
        // ID for Invoice 
        protected string ID { get; set; }
        protected int quantity { get; set; }
        protected string checkout_type { get; set; }
        protected string first_name { get; set; }
        protected string last_name { get; set; }
        protected string email { get; set; }
        protected string address1 { get; set; }
        protected string address2 { get; set; }
        protected string Message { get; set; }

        #region Set The Login
        //Main Login Method
        public void SetLogin(string user, string pass, string credentials, string client_id, string client_secret, string scope, string RandomString)
        {
            //Setters
            this.user = user;
            this.pass = pass;
            this.credentials = credentials;
            this.client_id = client_id;
            this.client_secret = client_secret;
            this.scope = scope;
            this.state = RandomString;
        }

        #endregion

        #region Access Token Genreation Method

        //Method For Getting Access Token
        public string GetAccessToken()
        {
                string url = "https://api.aligncommerce.com/oauth/access_token";
                var client = new RestSharp.RestClient();
                client.BaseUrl = url;
                client.Authenticator = new HttpBasicAuthenticator(user, pass);
                var request = new RestSharp.RestRequest();
                request.AddParameter("grant_type", this.credentials);
                request.AddParameter("client_id", this.client_id);
                request.AddParameter("client_secret", this.client_secret);
                request.AddParameter("scope", this.scope);
                request.AddParameter("state", this.state);

                IRestResponse response = client.Execute(request);
                //Deserialize Responce Object
                if (response.StatusCode.ToString() == "200")
                {
                    var js = new JavaScriptSerializer();
                    var dic = js.Deserialize<Dictionary<string, string>>(response.Content);
                    this.access_token = dic["access_token"];
                    return dic["access_token"];

                }
                else if (response.StatusCode.ToString() == "400" || response.StatusCode.ToString() == "401" || response.StatusCode.ToString() == "402" || response.StatusCode.ToString() == "403" || response.StatusCode.ToString() == "404" || response.StatusCode.ToString() == "500" || response.StatusCode.ToString() == "500")
                {
                    var js = new JavaScriptSerializer();
                    var dic = js.Deserialize<Dictionary<string, string>>(response.Content);
                    this.Message = dic["error_message"];
                    return Message;
                }
                else
                {
                    var js = new JavaScriptSerializer();
                    var dic = js.Deserialize<Dictionary<string, string>>(response.Content);
                    this.Message = dic["access_token"];
                    return Message;
                }   
        }
        #endregion

        #region Properties for Product
        //Required. Provided in Oauth flow. 
        //Required. Product name should be unique.
       protected  string product_name { get; set; }
        //product description
        protected string product_description { get; set; }
        //Merchant's sku for the product.
        protected string product_sku { get; set; }
        //Required. A numeric / float value that represents the price of the product. 
        protected Double product_price { get; set; }
        //A numeric / float value that represents how much the cost if the product would be shipped to the buyer.
        protected Double product_shipping { get; set; }
        //response
        protected string[] data { get; set; } 
        #endregion       
        #region Product Methods

        public string ListProduct(string AccessToken)
        {
            this.access_token = AccessToken;
            try
            {
                string url = "https://api.aligncommerce.com/products";
                var client = new RestSharp.RestClient();
                client.BaseUrl = url;
                client.Authenticator = new HttpBasicAuthenticator(user, pass);
                var request = new RestSharp.RestRequest();
                request.Method = Method.GET;
                //  request.Resource = json;
                request.AddParameter("access_token", this.access_token);

                IRestResponse response = client.Execute(request);
                if (response.StatusCode.ToString() == "200")
                {
                    return response.Content;
                }
                else
                {
                    return response.Content;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    
        #endregion
        #region Create Product
        public string CreateProduct(string access_token,string product_name,string product_discription,double product_price,double product_shipping,string product_sku)
        {
            // access_token string Required. Provided in Oauth flow. 
            // product_name string Required. Product name should be unique.
            // product_price numeric / float Required. A numeric / float value that represents the price of the product. 
            // product_shipping numeric / float A numeric / float value that represents how much the cost if the product would be shipped to the buyer. 
            // product_sku string Merchant's sku for the product. 

            this.access_token = access_token;
            this.product_name = product_name;
            this.product_price = product_price;
            this.product_shipping = product_shipping;
            this.product_sku = product_sku;
            this.product_description = product_discription;
            try
            {
                string url = "https://api.aligncommerce.com/products";
                var client = new RestSharp.RestClient();
                client.BaseUrl = url;
                client.Authenticator = new HttpBasicAuthenticator(user, pass);
                var request = new RestSharp.RestRequest();
                request.Method = Method.POST;
                //  request.Resource = json;
                request.AddParameter("access_token", this.access_token);
                request.AddParameter("product_name", this.product_name);
                request.AddParameter("product_price", this.product_price);
                request.AddParameter("product_shipping", this.product_shipping);
                request.AddParameter("product_sku", this.product_sku);
                request.AddParameter("product_description", this.product_description);
                IRestResponse response = client.Execute(request);
                //  return response.Content.ToString();
                
                if (response.StatusCode.ToString() == "200")
                {
                    var js = new JavaScriptSerializer();
                    var dic = js.Deserialize<Dictionary<string, string>>(response.Content);
                    this.Message = dic["message"];
                    return Message;
                }

                else if (response.StatusCode.ToString() == "400" || 
                            response.StatusCode.ToString() == "401" || 
                            response.StatusCode.ToString() == "402" || 
                            response.StatusCode.ToString() == "403" || 
                            response.StatusCode.ToString() == "404" || 
                            response.StatusCode.ToString() == "500" || 
                            response.StatusCode.ToString() == "502")
                {
                    var js = new JavaScriptSerializer();
                    var dic = js.Deserialize<Dictionary<string, string>>(response.Content);
                    this.Message = dic["error_message"];
                    return Message;
                }

                else
                {
                    return response.Content;
                }
                
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        #endregion
        #region Update Product
        public string UpdateProduct(string access_token
                                    , string updateid
                                    , string product_name
                                    , string product_discription
                                    , double product_price
                                    , double product_shipping
                                    , string product_sku)
        {        
           this.access_token = access_token;
           this.product_name = product_name;
           this.product_sku = product_sku;
           this.product_price = product_price;
           this.product_shipping = product_shipping;
           this.product_description = product_discription;
           this.ID = updateid;
        
            try
            {
                string url = "https://api.aligncommerce.com/products/id";
                var client = new RestSharp.RestClient();
                client.BaseUrl = url;
                client.Authenticator = new HttpBasicAuthenticator(user, pass);
                var request = new RestSharp.RestRequest();
                request.Method = Method.PUT;
                //  request.Resource = json;
                request.AddParameter("access_token", this.access_token);
                request.AddParameter("product_id",this.ID);
                request.AddParameter("product_name", this.product_name);
                request.AddParameter("product_price", this.product_price);
                request.AddParameter("product_shipping", this.product_shipping);
                request.AddParameter("product_sku", this.product_sku);

                IRestResponse response = client.Execute(request);
                if (response.StatusCode.ToString() == "200")
                {
                    var js = new JavaScriptSerializer();
                    var dic = js.Deserialize<Dictionary<string, string>>(response.Content);
                    this.Message = dic["message"];
                    return Message;
                }
                else if (response.StatusCode.ToString() == "400" || response.StatusCode.ToString() == "401" || response.StatusCode.ToString() == "402" || response.StatusCode.ToString() == "403" || response.StatusCode.ToString() == "404" || response.StatusCode.ToString() == "500" || response.StatusCode.ToString() == "500")
                {
                    var js = new JavaScriptSerializer();
                    var dic = js.Deserialize<Dictionary<string, string>>(response.Content);
                    this.Message = dic["error_message"];
                    return Message;
                }
                else
                {
                    var js = new JavaScriptSerializer();
                    var dic = js.Deserialize<Dictionary<string, string>>(response.Content);
                    this.Message = dic["error_message"];
                    return Message;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        #endregion

        #region List ALL Invoice
        public string GetInvoieALL(string token)
        {
            this.access_token = token;
            try
            {
                string url = "https://api.aligncommerce.com/invoice/";
                var client = new RestSharp.RestClient();
                client.BaseUrl = url;
                client.Authenticator = new HttpBasicAuthenticator(user, pass);
                var request = new RestSharp.RestRequest();
                request.Method = Method.GET;
                request.AddParameter("access_token", this.access_token);
                //  request.Resource = json;
                IRestResponse response = client.Execute(request);
                if (response.StatusCode.ToString() == "200")
                {
                    return response.Content;
                }
                else if (response.StatusCode.ToString() == "400" || response.StatusCode.ToString() == "401" || response.StatusCode.ToString() == "402" || response.StatusCode.ToString() == "403" || response.StatusCode.ToString() == "404" || response.StatusCode.ToString() == "500" || response.StatusCode.ToString() == "500")
                {
                    var js = new JavaScriptSerializer();
                    var dic = js.Deserialize<Dictionary<string, string>>(response.Content);
                    this.Message = dic["error_message"];
                    return Message;
                }
                else
                {
                    return response.Content;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        #endregion

        //for Invoice
        #region List Specific Invoice
        public string ListSpecificInvoice(string token, string id)
        {
            this.access_token = token;
            this.ID = id;
            try
            {
                string url = "https://api.aligncommerce.com/invoice/retrieve/id";
                var client = new RestSharp.RestClient();
                client.BaseUrl = url;
                client.Authenticator = new HttpBasicAuthenticator(user, pass);
                var request = new RestSharp.RestRequest();
                request.Method = Method.GET;
                request.AddParameter("access_token", this.access_token);
                request.AddParameter("id", this.ID);
                //  request.Resource = json;
                IRestResponse response = client.Execute(request);
                if (response.StatusCode.ToString() == "200")
                {
                    var js = new JavaScriptSerializer();
                    var dic = js.Deserialize<Dictionary<string, string>>(response.Content);
                    this.Message = dic["message"];
                    return Message;
                }
                else if (response.StatusCode.ToString() == "400" || response.StatusCode.ToString() == "401" || response.StatusCode.ToString() == "402" || response.StatusCode.ToString() == "403" || response.StatusCode.ToString() == "404" || response.StatusCode.ToString() == "500" || response.StatusCode.ToString() == "500")
                {
                    var js = new JavaScriptSerializer();
                    var dic = js.Deserialize<Dictionary<string, string>>(response.Content);
                    this.Message = dic["error_message"];
                    return Message;
                }
                else
                {
                    var js = new JavaScriptSerializer();
                    var dic = js.Deserialize<Dictionary<string, string>>(response.Content);
                    this.Message = dic["error_message"];
                    return Message;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        #endregion
        #region Create Invoice

        public string CreateInvoice(string access_token,string Checkout_Type,string product_name,double product_price,int quantity,double product_shipping,string firstname,string lastname,string address1,string address2)
        {
            
            this.access_token = access_token;
            this.checkout_type = Checkout_Type;
            this.product_name = product_name;
            this.product_price = product_price;
            this.quantity = quantity;
            this.product_shipping = product_shipping;
            this.first_name = firstname;
            this.last_name = lastname;
            this.address1 = address1;
            this.address2 = address2;

            XElement tk = new XElement("access_token", this.access_token);
            XElement checkoutty = new XElement("checkout_type", this.checkout_type);
            XElement prod = new XElement("products",
                                              new XElement("product_name", this.product_name),
                                              new XElement("product_price", this.product_price),
                                              new XElement("quantity", this.quantity),
                                              new XElement("product_shipping", this.product_shipping)
                                                       );

            XElement buyer = new XElement("buyer_info",
                                  new XElement("first_name", this.first_name),
                                  new XElement("last_name", this.last_name),
                                  new XElement("address_1", this.address1),
                                  new XElement("address_2", this.address2)
                                           );
                string url = "https://api.aligncommerce.com/invoice/create";
                var client = new RestSharp.RestClient();
                client.BaseUrl = url;
                client.Authenticator = new HttpBasicAuthenticator(user, pass);
                var request = new RestSharp.RestRequest();
                request.Method = Method.GET;
               request.AddParameter("access_token", this.access_token);
               request.AddParameter("product_name", this.product_name);
               request.AddParameter("product_price", this.product_price);
               request.AddParameter("quantity", this.quantity);
               request.AddParameter("product_shipping", this.product_shipping);
               request.AddParameter("first_name", this.first_name);
               request.AddParameter("last_name", this.last_name);
               request.AddParameter("address_1", this.address1);
               request.AddParameter("address_2", this.address2);
                //  request.Resource = json;
                IRestResponse response = client.Execute(request);
                if (response.StatusCode.ToString() == "200")
                {
                   return response.Content;
                }
                else
                {
                    return response.Content;
                }   
        }

        #endregion

        #region Buyer
        public string ListALLBuyer(string token)
        {
            this.access_token = token;

            try
            {
                string url = "https://api.aligncommerce.com/buyer";
                var client = new RestSharp.RestClient();
                client.BaseUrl = url;
                client.Authenticator = new HttpBasicAuthenticator(user, pass);
                var request = new RestSharp.RestRequest();
                request.Method = Method.GET;
                request.AddParameter("access_token", this.access_token);
                //  request.Resource = json;
                IRestResponse response = client.Execute(request);
                if (response.StatusCode.ToString() == "200")
                {
                   return response.Content;
                }
                else
                {
                    return response.Content;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }


        #region specific buyer
        public string GetSpecificBuyer(string token, string id)
        {
            this.access_token = token;
            this.ID = id;
            try
            {
                string url = "https://api.aligncommerce.com/invoice/retrieve/id";
                var client = new RestSharp.RestClient();
                client.BaseUrl = url;
                client.Authenticator = new HttpBasicAuthenticator(user, pass);
                var request = new RestSharp.RestRequest();
                request.Method = Method.GET;
                request.AddParameter("access_token", this.access_token);
                request.AddParameter("id", this.ID);
                //  request.Resource = json;
                IRestResponse response = client.Execute(request);
                if (response.StatusCode.ToString() == "200")
                {
                    return response.Content;
                }
                else
                {
                    return response.Content;
                }
                
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        #endregion
    }
    
    #endregion

    #endregion

}
