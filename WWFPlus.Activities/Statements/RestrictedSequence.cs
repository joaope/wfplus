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

namespace WWFPlus.Activities.Statements
{
    using System;
    using System.Activities;
    using System.Collections.ObjectModel;
    using System.Activities.Validation;
    using System.Globalization;

    public abstract class RestrictedSequence : NativeActivity, ISequenceContainer
    {
        public Collection<Activity> Activities { get; private set; }

        public Collection<Variable> Variables { get; private set; }

        public abstract Collection<Type> AllowedTypes { get; }

        public abstract Collection<Type> RestrictedTypes { get; }

        private Variable<int> CurrentActivityIndex { get; set; }

        private CompletionCallback OnChildCompletion { get; set; }

        protected RestrictedSequence()
        {
            CurrentActivityIndex = new Variable<int>();

            Activities = new Collection<Activity>();
            Variables = new Collection<Variable>();
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            metadata.AddImplementationVariable(CurrentActivityIndex);

            if (AllowedTypes != null &&
                AllowedTypes.Count > 0) 
            {
                foreach (var activity in Activities)
                {
                    foreach (var allowedType in AllowedTypes)
                    {
                        if (!allowedType.IsAssignableFrom(activity.GetType()))
                        {
                            metadata.AddValidationError(
                                string.Format(
                                    CultureInfo.InvariantCulture,
                                    "Activity '{0}' isn't allowed to be used inside '{0}'",
                                    activity.DisplayName,
                                    DisplayName));
                            break;
                        }
                    }
                }
            }
            else if (RestrictedTypes != null &&
                RestrictedTypes.Count > 0)
            {
                foreach (var activity in Activities)
                {
                    foreach (var restrictedType in RestrictedTypes)
                    {
                        if (restrictedType.IsAssignableFrom(activity.GetType()))
                        {
                            metadata.AddValidationError(
                                string.Format(
                                    CultureInfo.InvariantCulture,
                                    "Activity '{0}' isn't allowed to be used inside '{0}'",
                                    activity.DisplayName,
                                    DisplayName));
                            break;
                        }
                    }
                }
            }
        }

        protected override void Execute(NativeActivityContext context)
        {
            InternalExecution(context, null);
        }

        private void InternalExecution(
            NativeActivityContext context,
            ActivityInstance activityInstance)
        {
            int index = CurrentActivityIndex.Get(context);

            if (index == Activities.Count)
            {
                return;
            }

            if (OnChildCompletion == null)
            {
                OnChildCompletion = new CompletionCallback(InternalExecution);
            }

            context.ScheduleActivity(Activities[index], OnChildCompletion);

            CurrentActivityIndex.Set(context, ++index);
        }
    }
}

