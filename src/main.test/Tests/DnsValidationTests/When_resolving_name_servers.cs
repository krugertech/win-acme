﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nager.PublicSuffix;
using PKISharp.WACS.Clients.DNS;
using System.Linq;
using LogService = PKISharp.WACS.UnitTests.Mock.Services.LogService;

namespace PKISharp.WACS.UnitTests.Tests.DnsValidationTests
{
    [TestClass]
	public class When_resolving_name_servers
	{
		private readonly LookupClientProvider _dnsClient;

		public When_resolving_name_servers()
		{
			var webTldRuleProvider = new WebTldRuleProvider();
			var domainParser = new DomainParser(webTldRuleProvider);
			var log = new LogService(true);
            _dnsClient = new LookupClientProvider(domainParser, log);
		}

		[TestMethod]
		[DataRow("_acme-challenge.logs.hourstrackercloud.com", "Tx1e8X4LF-c615tnacJeuKmzkRmScZzsU-MJHxdDMhU")]
		[DataRow("_acme-challenge.candell.org", "PVyGjIMLGq9AnlKFvIX1aeSABFVmjbBvpez1_405ByI")]
		[DataRow("_acme-challenge.candell.org", "RwXi-dahnVtbNwzS9N9iUoC70o2S14ikGc70ofnKjZw")]
		public void Should_recursively_follow_cnames(string challengeUri, string expectedToken)
		{
			var tokens = _dnsClient.DefaultClient.GetTextRecordValues(challengeUri);
			Assert.IsTrue(tokens.Contains(expectedToken));
		}
	}
}