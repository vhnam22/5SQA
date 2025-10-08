package org.threeten.bp.zone;

import java.util.Iterator;
import java.util.ServiceConfigurationError;
import java.util.ServiceLoader;
import java.util.concurrent.atomic.AtomicBoolean;
import java.util.concurrent.atomic.AtomicReference;
import org.threeten.bp.chrono.JapaneseEra$$ExternalSyntheticBackportWithForwarding0;

public abstract class ZoneRulesInitializer {
    public static final ZoneRulesInitializer DO_NOTHING = new DoNothingZoneRulesInitializer();
    private static final AtomicBoolean INITIALIZED = new AtomicBoolean(false);
    private static final AtomicReference<ZoneRulesInitializer> INITIALIZER = new AtomicReference<>();

    /* access modifiers changed from: protected */
    public abstract void initializeProviders();

    public static void setInitializer(ZoneRulesInitializer initializer) {
        if (INITIALIZED.get()) {
            throw new IllegalStateException("Already initialized");
        } else if (!JapaneseEra$$ExternalSyntheticBackportWithForwarding0.m(INITIALIZER, (Object) null, initializer)) {
            throw new IllegalStateException("Initializer was already set, possibly with a default during initialization");
        }
    }

    static void initialize() {
        if (!INITIALIZED.getAndSet(true)) {
            AtomicReference<ZoneRulesInitializer> atomicReference = INITIALIZER;
            JapaneseEra$$ExternalSyntheticBackportWithForwarding0.m(atomicReference, (Object) null, new ServiceLoaderZoneRulesInitializer());
            atomicReference.get().initializeProviders();
            return;
        }
        throw new IllegalStateException("Already initialized");
    }

    static class DoNothingZoneRulesInitializer extends ZoneRulesInitializer {
        DoNothingZoneRulesInitializer() {
        }

        /* access modifiers changed from: protected */
        public void initializeProviders() {
        }
    }

    static class ServiceLoaderZoneRulesInitializer extends ZoneRulesInitializer {
        ServiceLoaderZoneRulesInitializer() {
        }

        /* access modifiers changed from: protected */
        public void initializeProviders() {
            Iterator i$ = ServiceLoader.load(ZoneRulesProvider.class, ZoneRulesProvider.class.getClassLoader()).iterator();
            while (i$.hasNext()) {
                try {
                    ZoneRulesProvider.registerProvider(i$.next());
                } catch (ServiceConfigurationError ex) {
                    if (!(ex.getCause() instanceof SecurityException)) {
                        throw ex;
                    }
                }
            }
        }
    }
}
