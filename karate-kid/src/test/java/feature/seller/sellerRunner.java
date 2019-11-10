package feature.seller;

import com.intuit.karate.junit5.Karate;

public class sellerRunner {

    // this will run all *.feature files that exist in sub-directories
    // see https://github.com/intuit/karate#naming-conventions
    @Karate.Test
    Karate testAll() {
        return new Karate().relativeTo(getClass());
    }

    @Karate.Test
    Karate testDone() {return new Karate().feature("seller").tags("done").relativeTo(getClass());}
}
