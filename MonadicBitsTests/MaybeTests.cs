using System;
using MonadicBits;
using NUnit.Framework;
using static MonadicBitsTests.TestMonads;

namespace MonadicBitsTests
{
    public static class MaybeTests
    {
        [Test]
        public static void Just_creates_maybe_with_value()
        {
            const string input = "Test";
            input.Just().Match(s => Assert.AreEqual(input, s), Assert.Fail);
        }

        [Test]
        public static void Nothing_creates_empty_maybe() =>
            Nothing<string>().Match(Assert.Fail, Assert.Pass);

        [Test]
        public static void Match_with_null_just_action_throws_exception() =>
            Assert.Throws<ArgumentNullException>(() => "Test".Just().Match(null, () => { }));

        [Test]
        public static void Match_with_null_nothing_action_throws_exception() =>
            Assert.Throws<ArgumentNullException>(() => "Test".Just().Match(_ => { }, null));

        [Test]
        public static void Match_with_null_just_func_throws_exception() =>
            Assert.Throws<ArgumentNullException>(() => "Test".Just().Match(null, () => "Nothing"));

        [Test]
        public static void Match_with_null_nothing_func_throws_exception() =>
            Assert.Throws<ArgumentNullException>(() => "Test".Just().Match(_ => "Just", null));

        [Test]
        public static void Match_maybe_with_value_returns_value()
        {
            const string value = "Test";
            Assert.AreEqual(value, value.Just().Match(v => v, () => "Nothing"));
        }

        [Test]
        public static void Match_empty_maybe_returns_nothing_value()
        {
            const string value = "Test";
            Assert.AreEqual(value, Nothing<string>().Match(_ => "Just", () => value));
        }

        [Test]
        public static void Match_maybe_with_value_calls_just_action() =>
            WithValue().Match(_ => Assert.Pass(), Assert.Fail);

        [Test]
        public static void Match_empty_maybe_calls_nothing_action() =>
            Nothing<string>().Match(Assert.Fail, Assert.Pass);

        [Test]
        public static void Map_with_null_mapping_throws_exception() =>
            Assert.Throws<ArgumentNullException>(() => "Test".Just().Map((Func<string, string>) null));

        [Test]
        public static void Map_maybe_with_value_returns_maybe_with_new_type_and_value()
        {
            const int mappedValue = 42;
            "Test".Just().Map(_ => mappedValue).Match(i => Assert.AreEqual(mappedValue, i), Assert.Fail);
        }

        [Test]
        public static void Map_empty_maybe_returns_empty_maybe() =>
            Nothing<string>().Map(_ => 42).Match(_ => Assert.Fail(), Assert.Pass);

        [Test]
        public static void Bind_to_null_method_throws_exception() =>
            Assert.Throws<ArgumentNullException>(() => "Test".Just().Bind((Func<string, Maybe<string>>) null));

        [Test]
        public static void Bind_maybe_with_value_to_method_returns_maybe()
        {
            const int bindValue = 42;
            "Test".Just().Bind(_ => bindValue.Just()).Match(i => Assert.AreEqual(bindValue, i), Assert.Fail);
        }

        [Test]
        public static void Bind_empty_maybe_to_method_returns_empty_maybe() =>
            Nothing<string>().Bind(_ => 42.Just()).Match(_ => Assert.Fail(), Assert.Pass);

        [Test]
        public static void Just_to_either_makes_right()
        {
            const string input = "Test";
            var result = input.Just().ToEither("Left");
            result.Match(Assert.Fail, right => Assert.AreEqual(input, right));
        }

        [Test]
        public static void Nothing_to_either_makes_left()
        {
            const string leftInput = "Left";
            var result = Nothing<string>().ToEither(leftInput);
            result.Match(left => Assert.AreEqual(leftInput, left), Assert.Fail);
        }
    }
}