using System;
using Xunit;
using Models;

public class ORMTests
{
    [Fact]
    public static void Record_Should_Succeed()
    {
        Testable testItem = new( //TODO replace with randomly generated objects
            "String Part",
            true,
            5.9f);
        Assert.True(testItem.Record(), "Record() should have recorded successfully and returned true");
    } 

    [Fact]
    public static void getAll_Should_Fetch_Recorded_Item_and_delete_should_succeed()
    {
         Testable testItem = new(
            "String Part",
            true,
            5.9f);
        testItem.Record();
        List<Testable>? fetchedItems = Testable.getAll(Testable.table);

        Assert.True(fetchedItems != null, "getAll() should not return null as an item was just recorded");

        Testable fetchedTestItem = fetchedItems.Last();

        Assert.True(
            fetchedTestItem.StrField == testItem.StrField &&
            fetchedTestItem.BoolField == testItem.BoolField &&
            fetchedTestItem.SingleField == testItem.SingleField,
            "Item fetched from getAll() should be identical to the item recorded"
        );

        Testable DelItem = new(
            "This Item Should Be Deleted.",
            true,
            5.9f);

        DelItem.Record();
        Assert.False(DelItem.Id == null);
        int? DelItemId = DelItem.Id;
        DelItem.DeleteRecord();

        List<Testable>? fetchedItems2 = Testable.getAll(Testable.table);
        Assert.False(fetchedItems2 == null, "Delete funtion should have preserved the existent items");
        Assert.False(fetchedItems2.Last().Id == DelItemId);
    }
}