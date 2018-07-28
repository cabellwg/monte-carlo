# Tax Planning

> This web application models the performance of a sample portfolio based on scenarios generated by Monte Carlo methods. It was built for the [PIEtech, Inc](https://www.moneyguidepro.com/ifa/). internship in summer 2018, as the third of three projects. It was built by a team of five interns: two business/technical analysts, one front-end developer, and me, a back-end developer.

---

#### Built with:

* ASP.NET Core MVC Web API
* [Aurelia](https://aurelia.io/)

---

#### My contributions:
* Built entire back-end API with .NET Core
* Designed Monte Carlo methods to forecast financial markets with [stochastic calculus](https://en.wikipedia.org/wiki/Stochastic_calculus).
* Helped with front-end code, building charts and assisting the front-end developer towards the end of the project in delivering the product on time


#### Basic structure:

* Back-end code can be found in the `MonteCarlo` directory
* Front-end code can be found in the `web-app` directory

This code was never deployed; it was an intern project and the goal was to have a functional development build. The only way it has ever been run is a Webpack development server for the front-end and the Visual Studio built-in IIS Express server for the back-end. To run it on any other configuration, the fetch URL on the front-end will have to be changed, and, depending on the configuration, CORS may have to be reconfigured.

---

## Important Note

As with any project, there are things that I did that I could have done better. Upon completion of the project, we did a code review and professional developers were very helpful and pointed out several things that could have been improved. I have left it as-is in order to
1. Demonstrate progress in software development ability with the [first](https://gitlab.com/cabellwg/tax-planning) and [second](https://gitlab.com/cabellwg/guaranteed-income) projects and
2. Accurately demonstrate my software development skills in a limited period of development time (one three-week sprint).

---

## More details

The application takes a sample portfolio with amounts in stocks, bonds, and savings and models the performance of the portfolio. Two different Monte Carlo methods are used – one for stocks, one for bonds. Once a trial hits zero, it stops and the amounts remainder of the trial are also set to zero.

### Stocks

Stocks are modeled using [geometric Brownian motion](https://en.wikipedia.org/wiki/Geometric_Brownian_motion) (GBM), which is the motion of stock prices assumed by the [Black-Scholes model](https://en.wikipedia.org/wiki/Black%E2%80%93Scholes_model). True GBM, however, is modeled by the stochastic differential equation
```math
\text{d}S_t = \mu S_t\text{d}t + \sigma S_t \text{d}W_t
```
where $`S_t`$ is the stock price at time $`t`$, $`\mu`$ is a drift coefficient, $`\sigma`$ is a volatility coefficient, and $`W_t`$ is a [Wiener process](https://en.wikipedia.org/wiki/Wiener_process). Since we step in one-year increments, we use [Milstein's method](http://www.maths.lth.se/matstat/kurser/fmsn25masm24/lab2/finstat_ch11.pdf) to approximate the SDE as
```math
S_{t + 1} = S_t \left( 1 + \mu + \sigma W_t + \frac{1}{2}\sigma^2\left(W_t^2 - 1\right)\right).
```
We also discretize the Weiner process $`W_t`$ as a random walk with the steps distrubuted as a normal distribution, a Laplace distribution, or a Logistic distribution. A Gaussian random walk (a random walk with normal step distribution) will approach a Wiener process as the number of steps tends to infinity.

Values for the drift and volatility coefficients were calculated by the business analysts based on several historical datasets of several different types of stocks. The application shows projections based on historical data (from either 1928 or 1975) alongside projections based on "projected" data (data since 2000).

The GBM model models stock prices directly.

### Bonds

To project bond performance, we use a random walk directly on the bond yields. The random walk has step distributions given by normal, Laplace, or logistic distributions.

Distribution scales were calculated by the business analysts based on several historical datasets of several different types of stocks. The application shows projections based on historical data (from either 1928 or 1975) alongside projections based on "projected" data (data since 2000).

Bond yields have an artificial floor at 0.03%. Although [Japan has been playing around with some interesting policies lately](https://www.bloomberg.com/quicktake/negative-interest-rates), we ultimately decided that bond yields below 0.03% were not widespread enough to warrant being included in the model. 0.03% was selected since it was the lowest value historically observed in the United States.

---

#### Endnotes:
* This was all done in the Q measure, and it probably should have been done in the P measure. Before this project neither I nor the business analysts on my team had any exposure to financial forecasting methods or even mathematical finance, so I stepped up as the math major on the team and found enough information to get a reasonable model working. Two days before the presentation date (and the end of the internship) I stumbled on the [mathematical finance Wikipedia page](https://en.wikipedia.org/wiki/Mathematical_finance), which explains the difference in fairly clear terms.
* You may wonder why the histograms for the stocks don't exactly match the distributions, and may be skewed significantly to one direction or another. That's actually a really cool quirk of the application that took us forever to figure out. I finally realized what was happening after about three hours of staring at the screen next to our business analysts. With a random walk, large negative values, more often than not, are preceded by other negative values. In unbounded cases, we see the expected distribution. However, when a lower bound (at $0.00) is placed on a trial and after the trial hits $0.00 it stops, those preceding negative rates my drive the stock account to zero before the large negative rates can occur. Therefore the distributions will always be skewed to the right. We believe that sometimes they are skewed so much to the right that the indexing on the histogram gets messed up and they actually appear to be skewed left.
