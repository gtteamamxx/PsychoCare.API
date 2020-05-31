using FluentAssertions;
using NUnit.Framework;
using PsychoCare.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.BDDfy;

namespace PsychoCare.API.Tests.ServicesTests
{
    [TestFixture]
    public class TokenServiceTests
    {
        private DateTime _expiresIn;
        private Exception _thrownError;
        private string _token;
        private int _userId;

        [Test]
        public void All_100_Generated_Tokens_Should_Be_Same()
        {
            List<string> generatedTokens = null;
            this.Given(x => Expires_In_Is_2021())
                    .And(x => UserId_Is_Random_Int())
                .When(x => Generate_Token_100_Times(out generatedTokens))
                .Then(x => No_Error_Should_Be_Thrown())
                    .And(x => All_Tokens_Should_Be_Same(generatedTokens))
                .BDDfy();
        }

        [Test]
        public void Decoding_Token_With_Default_Param_Should_Throw_Exception()
        {
            this.Given(x => Token_Is_Null())
                .When(x => Decode_Token())
                .Then(x => Error_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Decoding_Token_With_Not_Valid_Token_Should_Throw_Exception()
        {
            this.Given(x => Token_Is_Not_Valid())
                .When(x => Decode_Token())
                .Then(x => Error_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Decoding_UserId_From_Token_Should_Return_10()
        {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2Fw" +
                           "Lm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zaWQiOiIxMCIsImV4cCI6MCwia" +
                           "XNzIjoiUHN5Y2hvQ2FyZSIsImF1ZCI6IlBzeWNob0NhcmUifQ.rc2ARIWvgOy-eWR8sxk" +
                           "PQWck5UYuSCOXg5x7nkrtoas";

            this.Given(x => Token_Is(token))
                .When(x => Decode_Token())
                .Then(x => No_Error_Should_Be_Thrown())
                    .And(x => UserId_Should_Be_10())
                .BDDfy();
        }

        [Test]
        public void Generating_Token_With_Default_Values_Should_Not_Throw_Error()
        {
            this.Given(x => Expires_In_Is_Default())
                    .And(x => User_Id_Is_Default())
                .When(x => GenerateToken())
                .Then(x => No_Error_Should_Be_Thrown())
                .BDDfy();
        }

        [SetUp]
        public void Reset()
        {
            _thrownError = null;
            _token = null;
        }

        private void All_Tokens_Should_Be_Same(List<string> generatedTokens)
        {
            generatedTokens.Distinct().Count().Should().Be(1, "all generated tokens should be same");
        }

        private void Decode_Token()
        {
            try
            {
                _userId = new TokenService().GetUserIdFromToken(_token);
            }
            catch (Exception ex)
            {
                _thrownError = ex;
            }
        }

        private void Error_Should_Be_Thrown()
        {
            _thrownError.Should().NotBeNull();
        }

        private void Expires_In_Is_2021()
        {
            _expiresIn = new DateTime(2021);
        }

        private void Expires_In_Is_Default()
        {
            _expiresIn = default;
        }

        private void Generate_Token_100_Times(out List<string> tokens)
        {
            var tokenService = new TokenService();
            tokens = Enumerable.Range(0, 100).Select(x => tokenService.GenerateTokenForUser(_expiresIn, _userId)).ToList();
        }

        private void GenerateToken()
        {
            try
            {
                _token = new TokenService().GenerateTokenForUser(_expiresIn, _userId);
            }
            catch (Exception ex)
            {
                _thrownError = ex;
            }
        }

        private void No_Error_Should_Be_Thrown()
        {
            _thrownError.Should().BeNull();
        }

        private void Token_Is(string token)
        {
            _token = token;
        }

        private void Token_Is_Not_Valid()
        {
            _token = "BAD_TOKEN";
        }

        private void Token_Is_Null()
        {
            _token = null;
        }

        private void User_Id_Is_Default()
        {
            _userId = default;
        }

        private void UserId_Is_Random_Int()
        {
            _userId = 10;
        }

        private void UserId_Should_Be_10()
        {
            _userId.Should().Be(10);
        }
    }
}