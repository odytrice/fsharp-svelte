module Server.Tests

open System
open System.IO
open Xunit
open System.Threading.Tasks
open Microsoft.AspNetCore.Http

open Server.App
open Moq
open Giraffe

type TestMessage =
    {
        Text: string
    }

let createDefaultContext(): HttpContext =
    let mockServiceProvider = Mock<IServiceProvider>()
    let mockSerializer = Mock<Json.ISerializer>()
    mockServiceProvider.Setup(_.GetService(typeof<Json.ISerializer>)).Returns(mockSerializer.Object) |> ignore
    DefaultHttpContext(RequestServices = mockServiceProvider.Object)

let readResponseBody (ctx: HttpContext): Task<string> =
    task {
        ctx.Response.Body.Seek(0L, SeekOrigin.Begin) |> ignore
        use reader = new StreamReader(ctx.Response.Body)
        return! reader.ReadToEndAsync()
    }

let next : HttpFunc = fun _ -> Task.FromResult(None)

[<Fact>]
let ``indexHandler should return 200 OK`` () =
    task {
        // Arrange
        let ctx = createDefaultContext()

        // Act
        let! result = indexHandler next ctx

        // Assert
        match result with
        | Some ctx -> Assert.Equal(StatusCodes.Status200OK, ctx.Response.StatusCode)
        | None -> Assert.Fail "No Response"
    }

[<Fact>]
let ``indexHandler should have correct Content-Type`` () =
    task {
        // Arrange
        let ctx = createDefaultContext()

        // Act
        let! _ = indexHandler next ctx

        // Assert
        Assert.Equal("application/json; charset=utf-8", ctx.Response.ContentType)
    }
