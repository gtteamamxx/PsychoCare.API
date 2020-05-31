using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using PsychoCare.API.Filters;
using PsychoCare.API.Models;
using PsychoCare.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using TestStack.BDDfy;

namespace PsychoCare.API.Tests.FiltersTests
{
    [TestFixture]
    public class ErrorFilterTests
    {
        private bool _isExceptionHandled;
        private IActionResult _result;
        private int _statusCode;
        private Exception _throwedException;

        [Test]
        public void Filter_Should_Handle_Exception()
        {
            this.Given(x => Some_Exception())
                .When(x => Error_Filter_Catch())
                .Then(x => Exception_Should_Be_Handled())
                .BDDfy();
        }

        [SetUp]
        public void Reset()
        {
            _isExceptionHandled = false;
            _throwedException = null;
            _result = null;
            _statusCode = 0;
        }

        [Test]
        public void Status_Code_Should_Be_Set_To_500()
        {
            this.Given(x => Some_Exception())
               .When(x => Error_Filter_Catch())
               .Then(x => Status_Code_Is_500())
               .BDDfy();
        }

        [Test]
        public void Throwed_Message_Exception_Should_Return_Json_Response_With_Is_Error_Set_To_False()
        {
            this.Given(x => Some_Mesasge_Exception())
                .When(x => Error_Filter_Catch())
                .Then(x => Response_Is_Error_Should_Be_False())
                .BDDfy();
        }

        [Test]
        public void Throwed_Unhandled_Exception_Should_Return_Json_Response_With_Is_Error_Set_To_True()
        {
            this.Given(x => Some_Exception())
                .When(x => Error_Filter_Catch())
                .Then(x => Response_Is_Error_Should_Be_True())
                .BDDfy();
        }

        private void Error_Filter_Catch()
        {
            var errorFilter = new ErrorFilter();

            var context = new ExceptionContext(
                new ActionContext()
                {
                    HttpContext = new DefaultHttpContext(),
                    ActionDescriptor = new ActionDescriptor(),
                    RouteData = new RouteData()
                },
                new List<IFilterMetadata>()
            );
            context.Exception = _throwedException;

            errorFilter.OnException(context);

            _statusCode = context.HttpContext.Response.StatusCode;
            _result = context.Result;
            _isExceptionHandled = context.ExceptionHandled;
        }

        private void Exception_Should_Be_Handled()
        {
            _isExceptionHandled.Should().BeTrue();
        }

        private void Response_Is_Error_Should_Be_False()
        {
            _result.Should().BeOfType(typeof(JsonResult));

            ((_result as JsonResult).Value as ErrorFilterResponse).IsError.Should().BeFalse();
        }

        private void Response_Is_Error_Should_Be_True()
        {
            _result.Should().BeOfType(typeof(JsonResult));

            ((_result as JsonResult).Value as ErrorFilterResponse).IsError.Should().BeTrue();
        }

        private void Some_Exception()
        {
            _throwedException = new Exception();
        }

        private void Some_Mesasge_Exception()
        {
            _throwedException = new MessageException("Some message exception");
        }

        private void Status_Code_Is_500()
        {
            _statusCode.Should().Be(500);
        }
    }
}