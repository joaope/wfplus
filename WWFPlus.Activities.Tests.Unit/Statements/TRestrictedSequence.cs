#region License
// Copyright (C) 2011 by João Correia (http://wwfplus.codeplex.com/)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion

namespace WWFPlus.Activities.Tests.Unit
{
    using System;
    using System.Activities;
    using System.Activities.Expressions;
    using System.Activities.Statements;
    using System.Collections.ObjectModel;
    using System.IO;
    using NUnit.Framework;
    using WWFPlus.Activities.Statements;

    [TestFixture]
    public class TRestrictedSequence
    {
        class NoDelaySequence : RestrictedSequence
        {
            public override Collection<Type> AllowedTypes
            {
                get 
                {
                    return null;
                }
            }

            public override Collection<Type> RestrictedTypes
            {
                get
                {
                    return new Collection<Type>()
                    {
                        typeof(Delay)
                    };
                }
            }
        }

        class OnlyCodeActivitiesAndWriteLineSequence : RestrictedSequence
        {
            public override Collection<Type> AllowedTypes
            {
                get
                {
                    return new Collection<Type>()
                    {
                        typeof(WriteLine),
                        typeof(CodeActivity)
                    };
                }
            }

            public override Collection<Type> RestrictedTypes
            {
                get
                {
                    return null;
                }
            }
        }

        [Test]
        public void RestrictedSequence_T001()
        {
            var activity = new NoDelaySequence()
            {
                Activities =
                {
                    new Delay() { Duration = TimeSpan.FromHours(1) }
                }
            };

            var valResults = activity.Validate();

            Assert.That(
                valResults.Errors.Count, 
                Is.EqualTo(1));
        }

        [Test]
        public void RestrictedSequence_T002()
        {
            using (var writer = new StringWriter())
            {
                var activity = new NoDelaySequence()
                {
                    Activities =
                    {
                        new WriteLine() { Text = "foo" },
                        new WriteLine() { Text = "bar" },
                        new Add<int,int,int>() { Left = 1, Right = 2 }
                    }
                };

                var valResults = activity.Validate();

                var invoker = new WorkflowInvoker(activity);
                invoker.Extensions.Add(writer);
                invoker.Invoke();

                Assert.That(
                    valResults.Errors.Count,
                    Is.Not.GreaterThan(0));

                Assert.That(
                    valResults.Warnings.Count,
                    Is.Not.GreaterThan(0));

                Assert.That(
                    writer.ToString(),
                    Is.EqualTo(string.Concat(
                        "foo", 
                        Environment.NewLine, 
                        "bar",
                        Environment.NewLine)));
            }
        }

        [Test]
        public void RestrictedSequence_T003()
        {
            var activity = new OnlyCodeActivitiesAndWriteLineSequence()
            {
                Activities =
                {
                    new Delay() { Duration = TimeSpan.FromHours(1) },
                    new Sequence()
                }
            };

            var valResults = activity.Validate();

            Assert.That(
                valResults.Errors.Count,
                Is.EqualTo(2));
        }
    }
}

