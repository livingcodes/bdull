# BDull

Language that compiles to .NET CIL (Common Intermediate Language).

# Example

BDull

```
+Cms.User(+S First, +S Last, -D birth)
   S Name = $"{First} {Last}"
   I Age = calcAge(birth)
   -I calcAge(D birth)
      D now = DateTime.Now
      I yrs = now.Year - birth.Year
      D date = D(now.Year, birth.Month, birth.Day)
      if now > date then
         yrs--
      return yrs
```

C# equivalent

```
namespace Cms;
class User
{
   public readonly string First, Last;
   protected DateTime birth;
   public User(string First, string Last, DateTime birth)
   {
      this.First = First;
      this.Last = Last;
      this.birth = birth;
      Age = calcAge(birth);
   }
   public readonly int Age;

   int calcAge(DateTime birth) {
      var now = DateTime.Now;
      var yrs = (now.Year - birth.Year);
      var date = new DateTime(now.Year, birth.Month, birth.Day);
      if (now > date)
         yrs--;
      return yrs;
   }
}
```
