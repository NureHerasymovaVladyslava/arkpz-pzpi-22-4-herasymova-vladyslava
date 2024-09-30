#include <iostream>
#include <string>
#include <vector>

// поганий приклад
#define forever for (;;)

// гарний приклад
#define FOREVER for(;;)

// поганий приклад
void badNamingStyle() {
    int my_map1;
    int MyMap2;
    int MYMAP3;
}

// гарний приклад
void goodNamingStyle() {
    int my_map_1;
    int my_map_2;
    int my_map_3;
}


// поганий приклад
int f(int v1, int v2) {
    int v3 = v1 / v2;
    return v3;
}

// гарний приклад
int division(int divident, int divider) {
    int result = divident / divider;
    return result;
}


// поганий приклад
void badReuseInNestedScopes() {
    int d = 0;
    if (true) {
        d = 9;
    }
    else {
        int d = 7; // повторне використання імені
        //...
        d = 5;
    }
    std::cout << d;
}

// гарний приклад
void goodAvoidReuseInNestedScopes() {
    int d = 0;
    if (true) {
        d = 9;
    }
    else {
        int e = 7;
        //...
        d = 5;
    }
    std::cout << d;
}


// поганий приклад
void print_int(int i) {
    std::cout << i << std::endl;
}

void print_string(std::string s) {
    std::cout << s << std::endl;
}

// гарний приклад
void print(int i) {
    std::cout << i << std::endl;
}

void print(std::string s) {
    std::cout << s << std::endl;
}


// поганий приклад
void badEarlyDeclaration() {
    int i;  
    //...
    i = 7;   // пізня ініціалізація
    ///...
    std::cout << i;
}

// гарний приклад
void goodLateDeclaration() {
    int i = 7;
    std::cout << i;
}


// поганий приклад
void badLargeScope() {
    int i;
    for (i = 0; i < 10; ++i) {
        //...
    }
    // i все ще доступна
}

// гарний приклад
void goodSmallScope() {
    for (int i = 0; i < 10; ++i) {
        // i локальна для циклу
    }
}


// поганий приклад
void badVariableReuse() {
    int i;
    for (i = 0; i < 20; ++i) {
        //...
    }
    for (i = 0; i < 200; ++i) {
        //...
    }
}

// гарний приклад
void goodSeparateVariable() {
    for (int i = 0; i < 20; ++i) {
        //...
    }
    for (int j = 0; j < 200; ++j) {
        //...
    }
}


// поганий приклад
void badUngroupedData(int x1, int y1, int x2, int y2);

// гарний приклад
struct Point {
    int x;
    int y;
};

void goodGroupedData(Point from, Point to);


// поганий приклад
struct BadDeclaration {
    int x;
} bad_struct{ 2 };

// гарний приклад
struct GoodDeclaration {
    int x;
};
GoodDeclaration good_struct{ 2 };


// поганий приклад
class BadGetterSetter {
private:
    int x;
public:
    int getX() { return x; }
    void setX(int val) { x = val; }
};

// гарний приклад
class GoodClass {
public:
    int x;
};


// поганий приклад
void badOperator() {
    class X {
    public:
        int value;
        X operator+(const X& other) {
            return X{ value - other.value };
        }
    };
}

// гарний приклад
void goodOperator() {
    class X {
    public:
        int value;
        X operator+(const X& other) {
            return X{ value + other.value };
        }
    };
}


// поганий приклад
void badEnum() {
#define RED 0
#define BLUE 1
    int color = RED;  // Макрос замість enum
}

// поганий приклад
enum class BadColor { red = 1, blue = 2, green = 2 };

// гарний приклад
enum class Color { red, blue, green };
void goodEnumUsage() {
    Color color = Color::red;
}

// гарний приклад
enum class Month {
    jan = 1, feb, mar, apr, may, jun,
    jul, aug, sep, oct, nov, dec
};


// поганий приклад
void badIfElseSequence(int flag) {
    if (flag == 0) {
        //...
    }
    else if (flag == 1) {
        //...
    }
    else if (flag == 2) {
        //...
    }
    else {
        //...
    }
}

// гарний приклад
void goodSwitchUsage(int flag) {
    switch (flag) {
    case 0:
        //...
        break;
    case 1:
        //...
        break;
    case 2:
        //...
        break;
    default:
        //...
        break;
    }
}


// поганий приклад
void badForWhileUsage(std::vector<int> values) {
    for (int i = 0; i < values.size(); i++) {
        std::cout << values[i] << std::endl;
    }

    int j = 0;
    while (j < values.size()) {
        std::cout << values[j] << std::endl;
    }
}

// гарний приклад
void goodForUsage(std::vector<int> values) {
    for (auto v : values) {
        std::cout << v << std::endl;
    }
}


// поганий приклад
void badSwitchFallthrough(int eventType) {
    switch (eventType) {
    case 1:
        std::cout << "Warning\n";
        // Bad: implicit fallthrough
    case 2:
        std::cout << "Error\n";
        break;
    }
}

// гарний приклад
void goodSwitch(int eventType) {
    switch (eventType) {
    case 1:
        std::cout << "Warning\n";
        break;
    case 2:
        std::cout << "Error\n";
        break;
    default:
        std::cout << "Unknown\n";
        break;
    }
}


// поганий приклад
int badUnnecessaryExceptionUsage(std::vector<int> values, int v) {
    try {
        for (int i = 0; i < values.size(); i++) {
            if (values[i] == v) {
                throw i;
            }
        }
    }
    catch (int i) {
        return i;
    }

    return  -1;
}

// гарний приклад
int goodNoExceptions(std::vector<int> values, int v) {
    for (int i = 0; i < values.size(); i++) {
        if (values[i] == v) {
            return i;
        }
    }

    return  -1;
}


// поганий приклад
int badExceptionUsage(int a, int b) {
    if (b == 0) {
        throw "Division by zero";
    }
    return a / b;
}

// гарний приклад
class DivisionByZeroException : public std::runtime_error {
public:
    DivisionByZeroException()
        : std::runtime_error("Division by zero error") {}
};

int goodExceptionUsage(int a, int b) {
    if (b == 0) {
        throw DivisionByZeroException();
    }
    return a / b;
}


// поганий приклад
void badOperatorUsage(const unsigned int mask, 
    unsigned int value) {
    if (value & mask == 1) {
        std::cout << "Mask matches the value" << std::endl;
    }
    else {
        std::cout << "No match" << std::endl;
    }
}

// гарний приклад
void goodOperatorUsage(const unsigned int mask, 
    unsigned int value) {
    if ((value & mask) == 1) {
        std::cout << "Mask matches the value" << std::endl;
    }
    else {
        std::cout << "No match" << std::endl;
    }
}


// поганий приклад
void badMixingSignedAndUnsigned() {
    int v1 = -10;
    unsigned int v2 = 3;

    std::cout << v1 - v2 << std::endl;
    std::cout << v1 + v2 << std::endl;
    std::cout << v1 * v2 << std::endl;
}

// гарний приклад
void goodUsingSigned() {
    int v1 = -10;
    int v2 = 3;

    std::cout << v1 - v2 << std::endl;
    std::cout << v1 + v2 << std::endl;
    std::cout << v1 * v2 << std::endl;
}


// поганий приклад
void badSpaceUsage(int args [ ], int args_count) {
    for ( int i = 0; i < args_count; i ++ ) {

        std::cout << args [ i ] << std::endl;

    }


    for ( int i = 0; i < 100; i++ );
}

// гарний приклад
void goodSpaceUsage(int args[], int args_count) {
    for (int i = 0; i < args_count; i++) {
        std::cout << args[i] << std::endl;
    }

    for (int i = 0; i < 100; i++) {
        //nothing
    }
}


// поганий приклад
void readProcessAndPrint() {
    int a, b;
    std::cin >> a >> b;

    int sum = a + b;

    std::cout << "Sum: " << sum << std::endl;
}

// гарний приклад
int read(std::istream& is) {
    int x;
    is >> x;
    return x;
}

int process(int a, int b) {
    return a + b;
}

void print(std::ostream& os, int result) {
    os << "Sum: " << result << std::endl;
}


// поганий приклад
void badDuplication(bool flag) {
    if (flag) {
        std::cout << "True\n";
        std::cout << "Done\n";
    }
    else {
        std::cout << "False\n";
        std::cout << "Done\n";
    }
}

// гарний приклад
void goodRemoveDuplication(bool flag) {
    std::cout << (flag ? "True" : "False") << "\n";
    std::cout << "Done\n";
}