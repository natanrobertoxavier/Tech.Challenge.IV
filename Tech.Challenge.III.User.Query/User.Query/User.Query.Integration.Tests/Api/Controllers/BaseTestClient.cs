﻿using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace User.Query.Integration.Tests.Api.Controllers;
public abstract class BaseTestClient
{
    protected readonly HttpClient Client;
    protected readonly CustomWebApplicationFactory<Program> Factory;
    protected readonly string ControllerUri;

    protected BaseTestClient(
        string controllerUri)
    {
        ControllerUri = controllerUri;
        Factory = new CustomWebApplicationFactory<Program>();
        Client = Factory.CreateClient(new WebApplicationFactoryClientOptions());
    }

    protected static async Task<T> DeserializeResponse<T>(HttpResponseMessage response) =>
        JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());

    //protected async Task<string> Login(string email, string senha)
    //{
    //    var requisicao = new RequestEmailPasswordUserJson(email, senha);

    //    var resposta = await PostRequest("/api/v1/login", requisicao);

    //    await using var respostaBody = await resposta.Content.ReadAsStreamAsync();

    //    var responseData = await JsonDocument.ParseAsync(respostaBody);

    //    return responseData.RootElement.GetProperty("token").GetString();
    //}

    //protected async Task<HttpResponseMessage> PostRequest(string uri, object body)
    //{
    //    var jsonString = JsonConvert.SerializeObject(body);

    //    return await Client.PostAsync(uri, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    //}

    //protected async Task<HttpResponseMessage> PostRequest(string uri, object body, string token = "")
    //{
    //    AuthorizeRequest(token);

    //    var jsonString = JsonConvert.SerializeObject(body);

    //    return await Client.PostAsync(uri, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    //}

    //protected async Task<HttpResponseMessage> GetRequest(string uri, string token = "")
    //{
    //    AuthorizeRequest(token);

    //    return await Client.GetAsync(uri);
    //}

    //protected async Task<HttpResponseMessage> PutRequest(string uri, object body, string token = "")
    //{
    //    AuthorizeRequest(token);

    //    var jsonString = JsonConvert.SerializeObject(body);

    //    return await Client.PutAsync(uri, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    //}

    //protected async Task<HttpResponseMessage> DeleteRequest(string uri, string token = "")
    //{
    //    AuthorizeRequest(token);

    //    return await Client.DeleteAsync(uri);
    //}

    //private void AuthorizeRequest(string token)
    //{
    //    if (!string.IsNullOrEmpty(token))
    //    {
    //        if (Client.DefaultRequestHeaders.Contains("Authorization"))
    //            Client.DefaultRequestHeaders.Remove("Authorization");

    //        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    //    }
    //}
}
