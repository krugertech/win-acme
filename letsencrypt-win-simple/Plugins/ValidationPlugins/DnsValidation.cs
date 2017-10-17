﻿using ACMESharp;
using ACMESharp.ACME;
using LetsEncrypt.ACME.Simple.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsEncrypt.ACME.Simple.Plugins.ValidationPlugins
{
    abstract class DnsValidation : IValidationPlugin
    {
        public string ChallengeType => AcmeProtocol.CHALLENGE_TYPE_DNS;

        public abstract string Name { get; }
        public abstract string Description { get; }

        public Action<AuthorizationState> PrepareChallenge(Target target, AuthorizeChallenge challenge, string identifier, Options options, InputService input)
        {
            var dnsChallenge = challenge.Challenge as DnsChallenge;
            var record = dnsChallenge.RecordName;
            CreateRecord(target, identifier, record, dnsChallenge.RecordValue);
            Program.Log.Information("Answer should now be available at {answerUri}", record);
            return authzState => DeleteRecord(target, identifier, record);
        }

        /// <summary>
        /// Delete validation record
        /// </summary>
        /// <param name="recordName">where the answerFile should be located</param>
        public abstract void DeleteRecord(Target target, string identifier, string recordName);

        /// <summary>
        /// Create validation record
        /// </summary>
        /// <param name="recordName">where the answerFile should be located</param>
        /// <param name="token">the token</param>
        public abstract void CreateRecord(Target target, string identifier, string recordName, string token);

        /// <summary>
        /// Should this validation option be shown for the target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool CanValidate(Target target)
        {
            return true;
        }

        public abstract void Aquire(Options options, InputService input, Target target);
        public abstract void Default(Options options, Target target);

        /// <summary>
        /// Create instance for specific target
        /// </summary>
        /// <param name="options"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual IValidationPlugin CreateInstance(Target target)
        {
            return this;
        }

    }
}