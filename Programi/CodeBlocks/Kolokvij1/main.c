#include <stdio.h>
#include <stdlib.h>

int sumaRekurzivno(int niz[],int n){
    if(n < 1){
        return 0;
    }
    else{
        return niz[n-1] + sumaRekurzivno(niz,n-1);
    }
}
int main()
{
int n;
printf("Unesite broj");
scanf("%d",&n);
int niz[n];
for(int i = 0;i<n;i++){
    scanf("%d",&niz[i]);
}
printf("%d",sumaRekurzivno(niz,n));
}
