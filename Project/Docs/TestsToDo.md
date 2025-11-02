🧱 1. Basic Functionality
Record()

 Create a new object and call Record(), then call GetAll() — it should appear.

 Record multiple different objects — all should appear in GetAll().

 Record an object twice — ensure it doesn’t duplicate (depending on your intended behavior: overwrite or append).

 Record objects with:

Empty strings

Null values (if allowed)

Min/max numeric values

Special characters in strings (e.g. :, ;, \n)

Very long strings

GetAll()

 Returns an empty list when no records exist.

 Returns all previously recorded objects accurately.

 The returned objects’ fields match exactly what was recorded.

 Works consistently after restarting the program (i.e., persistence verified).

 Doesn’t duplicate entries after multiple calls.

⚙️ 2. Consistency & Integrity

 Idempotency: Calling Record() multiple times with identical data doesn’t corrupt or duplicate.

 Consistency: The number of records before and after writing matches expectation.

 Atomicity: If Record() partially fails (e.g., due to I/O), the file isn’t left corrupted.

 Isolation: One type’s records don’t appear in another type’s GetAll().

🧩 3. Schema Mapping

 Each property of the object is written and read correctly.

 Ensure delimiter parsing works if your file format uses : or similar.

 Ensure optional or new fields in class definitions don’t crash loading older records.

 If enums or DateTimes exist, check that they serialize and deserialize correctly.

💣 4. Edge & Error Handling

 Record an object with all fields empty or null.

 Record an object with extremely large numeric values.

 Record with special characters in key fields (e.g., ID = "a:b:c").

 Delete or corrupt the storage file, then call GetAll() (should fail gracefully).

 Verify that GetAll() doesn’t crash if file is empty or malformed.

🧮 5. Performance

 Record 1,000–10,000 objects in a loop — ensure performance is acceptable.

 Measure how fast GetAll() loads large datasets.

 Measure the memory footprint of loaded objects (sanity check).

🔁 6. Cross-Type Isolation

If your ORM handles multiple types (e.g. Product, Customer, Order):

 Product.Record() should not affect Customer.GetAll().

 Files or records are isolated per type.

 GetAll() for one type doesn’t throw if another type’s file is missing.

🧪 7. Persistence Across Sessions

 Record a few objects, close the app, restart, call GetAll(), and confirm persistence.

 Delete one record’s file manually — ORM should detect and handle gracefully.

 Ensure data is not cached incorrectly between runs.

🧰 8. Stability Tests

 Call Record() and GetAll() in random order repeatedly (stress test).

 Run in parallel threads if your ORM allows concurrent access — test for race conditions.

 Simulate interruption (force-stop mid-write) and verify data integrity after restart.